using SportFieldBooking.Data.Model;
using SportFieldBooking.Biz.Model.User;
using SportFieldBooking.Helper.Pagination;
using Microsoft.AspNetCore.Http;

namespace SportFieldBooking.Biz.User
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model);
        Task<View> GetAsync(long id);
        Task<Page<List>> GetListAsync(long pageNumber, int pageSize, HttpContext httpContext);
        Task DeleteAsync(long id);
        Task<View> UpdateAsync(Edit model);
        Task<Page<List>> SearchUsernameAsync(string username, long pageIndex, int pageSize);
        Task<Page<List>> FilterCreatedDateAsync(string date, string condition, long pageIndex, int pageSize);
        Task<View> UpdateBalanceAsync(long id, long amount);
        Task<CurrentUser> GetLoginAsync(string email);
        Task<Boolean> CheckCredentialsAsync(CurrentUser model, string password);
    }
}
