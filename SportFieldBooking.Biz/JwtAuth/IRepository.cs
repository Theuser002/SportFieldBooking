using Microsoft.AspNetCore.Http;
using SportFieldBooking.Biz.Model.JwtAuth;

namespace SportFieldBooking.Biz.JwtAuth
{
    public interface IRepository
    {
        Task<CurrentUserInfo> GetLoginAsync(HttpContext httpContext, string username);
        Task<Boolean> CheckCredentialsAsync(HttpContext httpContext, CurrentUserInfo model, string password);
        Task SaveRefreshTokenAsync(string username, string? refreshToken, DateTime? refreshTokenExpiryTime);
        Task<UserWithToken> GetUserWithTokenAsync(string username);
        string GetClaimValueFromToken(HttpContext httpContext, string claimName);
        public int GetRoleFromToken(HttpContext httpContext);
        Task<long> GetCurrentUserIdAsync(HttpContext httpContext);
    }
}
