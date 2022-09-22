using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.JwtAuth;
using SportFieldBooking.Biz.Model.User;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace SportFieldBooking.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IRepositoryWrapper _repository;

        public AuthenticateController(IConfiguration configuration, ILogger<UserController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }
        
        //[AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation($"User with username {model.Username} trying to log in");
                _logger.LogDebug("Validate model");
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                _logger.LogDebug($"Get user by username");
                var user = await _repository.JwtAuth.GetLoginAsync(HttpContext, model.Username);

                _logger.LogDebug($"Checking user's credentials");
                if (!await _repository.JwtAuth.CheckCredentialsAsync(HttpContext, user, model.Password))
                {
                    return Unauthorized();
                }

                var res = new LoginResponse
                {
                    Id = user.Id,
                    Code = user.Code,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role
                };

                var authClaims = new List<Claim>()
                {
                    new Claim("Id", $"{user.Id}"),
                    new Claim("Code", user.Code),
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim("Username", $"{user.Username}"),
                    new Claim("IsActive", $"{user.IsActive}"),
                    new Claim("Role", $"{user.Role}"),
                    new Claim(ClaimTypes.Role, $"{user.Role}")
                };

                _logger.LogDebug($"Generate user token(s)");
                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();
                _ = int.TryParse(_configuration["Jwt:RefreshTokenExpiration"], out int refreshTokenValidityInSeconds);
                var refreshTokenExpiryTime = DateTime.Now.AddSeconds(refreshTokenValidityInSeconds);
                
                res.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                res.accessTokenExpiryTime = token.ValidTo;
                res.RefreshToken = refreshToken;
                res.refreshTokenExpiryTime = refreshTokenExpiryTime;
                
                await _repository.JwtAuth.SaveRefreshTokenAsync(user.Username, refreshToken, refreshTokenExpiryTime);
                return Ok(res);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Unable to login user {model.Username}");
                return BadRequest(e.Message);
            }
        }

        //[HttpPost("Logout")]
        //public ActionResult Logout()
        //{
        //    var userName = User?.Identity?.Name;
        //    _logger.LogInformation($"User [{userName}] logged out of the system. ");
        //    try
        //    {
        //        _logger.LogDebug($"Get token");
        //        string token = HttpContent.
        //    }
        //}

        //[AllowAnonymous]
        [HttpPost("RefreshToken")]
        public async Task<IActionResult> RefreshToken(RefreshTokenRequest model)
        {
            try
            {
                if (model == null)
                {
                    return BadRequest("Invalid client request");
                }

                string? accessToken = model.AccessToken;
                string? refreshToken = model.RefreshToken;

                var principal = GetPrincipalFromExpiredToken(accessToken);
                if (principal == null)
                {
                    return BadRequest("Invalid token principal");
                }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
                string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                var userWithToken = await _repository.JwtAuth.GetUserWithTokenAsync(username);

                if (userWithToken == null || userWithToken.RefreshToken != refreshToken || userWithToken.refreshTokenExpiryTime <= DateTime.Now)
                {
                    return BadRequest("Expired refresh token or refresh token and access token mismatch");
                }

                var newAccessToken = CreateToken(principal.Claims.ToList());
                var newRefreshToken = GenerateRefreshToken();
                var tokenExpiryTime = userWithToken.refreshTokenExpiryTime;
                await _repository.JwtAuth.SaveRefreshTokenAsync(username, newRefreshToken, tokenExpiryTime);

                return new ObjectResult(
                    new
                    {
                        accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                        refreshToken = newRefreshToken
                    });
            }
            catch (Exception e)
            {
                _logger.LogError($"[My log]: Error refreshing token: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
            _ = int.TryParse(_configuration["Jwt:AccessTokenExpiration"], out int tokenValidityInSeconds);
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:ValidIssuer"],
                audience: _configuration["Jwt:ValidAudience"],
                expires: DateTime.Now.AddSeconds(tokenValidityInSeconds),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var rand = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(rand);
            return Convert.ToBase64String(rand);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid access token");
            }

            return principal;
        }
    }
}
