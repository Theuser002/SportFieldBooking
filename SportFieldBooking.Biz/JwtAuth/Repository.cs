using SportFieldBooking.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using SportFieldBooking.Biz.Model.JwtAuth;
using System.Security.Claims;

namespace SportFieldBooking.Biz.JwtAuth
{
    public class Repository : IRepository
    {
        private readonly DomainDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly ILogger<IRepositoryWrapper> _logger;
        private readonly IMapper _mapper;

        public Repository(DomainDbContext context, IConfiguration configuration, ILogger<RepositoryWrapper> logger, IMapper mapper)
        {
            _dbContext = context;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
        }

        // Get current information from username
        public async Task<CurrentUserInfo> GetLoginAsync(HttpContext httpContext, string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception($"There's no user with the username {username}");
            }
            var currentUser = _mapper.Map<CurrentUserInfo>(user);
            return currentUser;
        }

        // Check if the user input password equal to the true password of the user
        public async Task<Boolean> CheckCredentialsAsync(HttpContext httpContext, CurrentUserInfo model, string password)
        {
            return String.Equals(model.Password, password);
        }

        public async Task SaveRefreshTokenAsync(string username, string? refreshToken, DateTime? refreshTokenExpiryTime)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception($"No user with username {username}");
            }
            user.RefreshToken = refreshToken;
            user.refreshTokenExpiryTime = refreshTokenExpiryTime;
            await _dbContext.SaveChangesAsync();
        }

        public async Task<UserWithToken> GetUserWithTokenAsync(string username)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception($"No user with the username {username}");
            }
            var userWithToken = _mapper.Map<UserWithToken>(user);
            return userWithToken;
        }

        public string GetClaimValueFromToken (HttpContext httpContext, string claimName)
        {
            var identity = httpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                IEnumerable<Claim> claims = identity.Claims;
                var res = identity.FindFirst($"{claimName}")?.Value;
                if (res == null)
                {
                    throw new Exception($"No claim with {claimName} received from issuer");
                }
                return res;
            }
            throw new Exception($"Undefined User Identity");
        }

        public int GetRoleFromToken (HttpContext httpContext)
        {
            _ = int.TryParse(GetClaimValueFromToken(httpContext, "Role"), out var role);
            if (role == null)
            {
                throw new Exception("Error getting role from token");
            }
            return role;
        }

        public async Task<long> GetCurrentUserIdAsync(HttpContext httpContext)
        {
            var username = httpContext.User.Identity.Name;
            var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null)
            {
                throw new Exception($"Error getting user with username {username}");
            }
            return user.Id;
        }
    }
}
