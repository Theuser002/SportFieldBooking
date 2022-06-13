using Microsoft.AspNetCore.Http;
using SportFieldBooking.Data.Model;
using SportFieldBooking.Biz.Model.User;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.User
{
    public interface IRepository
    {
        Task<View> CreateAsync(HttpContext httpContext, New model, int role);
        Task<View> GetAsync(HttpContext httpContext, long id);
        Task<Page<List>> GetListAsync(HttpContext httpContext, long pageNumber, int pageSize);
        Task DeleteAsync(HttpContext httpContext, long id);
        Task<View> UpdateAsync(HttpContext httpContext, Edit model, long id);
        Task<Page<List>> SearchUsernameAsync(HttpContext httpContext, string username, long pageIndex, int pageSize);
        Task<Page<List>> FilterCreatedDateAsync(HttpContext httpContext, string date, string condition, long pageIndex, int pageSize);
        Task<View> UpdateBalanceAsync(HttpContext httpContext, long id, long amount);
        Task<List<List>> GetAllAsync();
    }
}
