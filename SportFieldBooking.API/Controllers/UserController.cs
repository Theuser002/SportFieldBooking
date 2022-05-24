using SportFieldBooking.Biz;
using SportFieldBooking.Biz.Model.User;
using Microsoft.AspNetCore.Mvc;
using SportFieldBooking.Helper.Exceptions;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace SportFieldBooking.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserController> _logger;
        private readonly IRepositoryWrapper _repository;

        public UserController(IConfiguration configuration, ILogger<UserController> logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _configuration = configuration;
            _repository = repository;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 18/04/2022
        /// Endpoint tao moi mot user (async)
        /// </summary
        /// <param name="model"> Biz model chua cac thong tin dau vao can thiet cho tao moi user </param>
        /// <returns> Response, tao user thanh cong thi tra ve response kem biz model cho view user, con k thi tra ve response loi </returns>
        /// <exception cref="Exception"> Khi tao user bi loi </exception>
        [HttpPost("CreateUser")]
        public async Task<IActionResult> Create(New model)
        {
            try
            {
                var item = await _repository.User.CreateAsync(model);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error creating user, {e}");
                return BadRequest(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 19/03/2022
        /// Endpoint lay thong tin mot user (async)
        /// </summary
        /// <param name="id"> Id cua user can lay </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="Exception"> Khi lay thong tin user bi loi </exception>
        [HttpGet("Get/{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var item = await _repository.User.GetAsync(id);
                return Ok(item);
            }
            catch (Exception e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 20/04/2022
        /// Endpoint lay thong tin cua user co phan trang (async)
        /// </summary>
        /// <param name="pageNumber"> So thu tu trang </param>
        /// <param name="pageSize"> So ban ghi trong mot trang </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        /// <exception cref="InvalidPageException"> Khi trang khong ton tai </exception>
        /// <exception cref="Exception"> Khi lay thong tin user co phan trang bi loi </exception>
        [HttpGet("GetList")]
        public async Task<IActionResult> GetList(long pageNumber, int pageSize)
        {
            try
            {
                Console.WriteLine($"Is User.Identity authenticated: {User.Identity.IsAuthenticated}");
                var items = await _repository.User.GetListAsync(HttpContext, pageNumber, pageSize);
                return Ok(items);
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error getting list of users in pages, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint xoa di mot user
        /// </summary>
        /// <param name="id"> id cua user </param>
        /// <returns>Response, thanh cong hoac loi</returns>
        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await _repository.User.DeleteAsync(id);
                return Ok();
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error deleting user, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Author: Hung
        /// Created: 25/04/2022
        /// Endpoint cap nhat thong tin mot user
        /// </summary>
        /// <param name="model"> Biz model chua cac thong tin dau vao can thiet cho update mot user </param>
        /// <returns> Response, thanh cong hoac loi </returns>
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> Update(Edit model)
        {
            try
            {
                var item = await _repository.User.UpdateAsync(model);
                return Ok(item);
            }
            catch(Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating user, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint tim kiem user voi username chua mot chuoi nhat dinh
        /// </summary>
        /// <param name="username"> chuoi dau vao </param>
        /// <param name="pageIndex"> so ban ghi trong mot trang </param>
        /// <param name="pageSize"> so user </param>
        /// <returns></returns>
        [HttpGet("SearchUsername")]
        public async Task<IActionResult> SearchUsername(string username, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.User.SearchUsernameAsync(username, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error searching for users, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Endpoint loc user dua vao ngay khoi tao
        /// </summary>
        /// <param name="date"> ngay duoc chon lam moc, co format: yyyy-mm-dd </param>
        /// <param name="condition"> dieu kien loc after: sau, before: truoc, equal: bang voi ngay lam moc </param>
        /// <param name="pageIndex"> so thu tu trang </param>
        /// <param name="pageSize"> so ban ghi trong mot trang </param>
        /// <returns> Cac nguoi dung duoc loc ra  </returns>
        [HttpGet("FilterByCreatedDate")]
        public async Task<IActionResult> FilterByCreatedDate(string date, string condition, long pageIndex, int pageSize)
        {
            try
            {
                var items = await _repository.User.FilterCreatedDateAsync(date, condition, pageIndex, pageSize);
                return Ok(items);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error filtering users by created date, {e}");
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 30/04/2022
        /// Endpoint cap nhat so du cua nguoi dung
        /// </summary>
        /// <param name="id"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        [HttpPut("UpdateBalance")]
        public async Task<IActionResult> UpdateBalance (long id, long amount)
        {
            try
            {
                var item = await _repository.User.UpdateBalanceAsync(id, amount);
                return Ok(item);
            }
            catch (Exception e)
            {
                _logger.LogError($"[MyLog]: Error updating the balance of user with the id {id}");
                return NotFound(e.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")] // [31]
        public async Task<IActionResult> Login([FromBody] LoginRequest model)
        {
            try
            {
                _logger.LogInformation($"User with email {model.Email} loggin in");
                _logger.LogDebug("Validate model");
                if (!ModelState.IsValid) // [33]
                {
                    return BadRequest();
                }

                _logger.LogDebug($"Get user by email");
                var user = await _repository.User.GetLoginAsync(model.Email);

                _logger.LogDebug($"Checking user's credentials");
                if (!await _repository.User.CheckCredentialsAsync(user, model.Password))
                {
                    return Unauthorized();
                }

                var result = new LoginResponse
                {
                    Id = user.Id,
                    Code = user.Code,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role
                };

                // Khong nen add cac thuoc tinh co the thay doi duoc (vi du: email) vao claim neu khong can dung den vi nhu the thi se phai reload de tao lai jwt token sau khi thay doi thuoc tinh do (vi du: reload lai trang sau khi nguoi dung thay doi email)
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
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:ValidIssuer"],
                    audience: _configuration["Jwt:ValidAudience"],
                    expires: DateTime.Now.AddDays(_configuration.GetValue<int>("Jwt:AccessTokenExpiration")),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                result.AccessToken = new JwtSecurityTokenHandler().WriteToken(token);
                result.tokenExpiration = token.ValidTo;
                return Ok(result);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error while logging in", e.Message);
                return BadRequest(e.Message);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            var userEmail = User.Identity;
        }

    }
}
