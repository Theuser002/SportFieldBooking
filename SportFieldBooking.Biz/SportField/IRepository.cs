using Microsoft.AspNetCore.Http;
using SportFieldBooking.Biz.Model.SportField;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.SportField
{
    public interface IRepository
    {
        Task<View> CreateAsync(HttpContext httpContext, New model);
        Task<View> GetAsync(HttpContext httpContext, long id);
        Task<Page<List>> GetListAsync(HttpContext httpContext, long pageIndex, int pageSize);
        Task DeleteAsync(HttpContext httpContext, long id);
        Task<View> UpdateAsync(HttpContext httpContext, Edit model);
        Task<Page<List>> SearchNameAsync(HttpContext httpContext, string name, long pageIndex, int pageSize);
        Task<Page<List>> FindOpeningAsync(HttpContext httpContext, long pageIndex, int pageSize);
        Task<Page<List>> FilterByTime(HttpContext httpContext, DateTime timeStart, DateTime timeEnd, long pageIndex, int pageSize);
    }
}
