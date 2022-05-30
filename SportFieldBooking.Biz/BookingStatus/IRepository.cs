using Microsoft.AspNetCore.Http;
using SportFieldBooking.Biz.Model.BookingStatus;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.BookingStatus
{
    public interface IRepository
    {
        Task<View> CreateAsync(HttpContext httpContext, New model);
        Task<View> GetAsync(HttpContext httpContext, long id);
        Task<Page<List>> GetListsAsync(HttpContext httpContext, long pageIndex, int pageSize);
        Task DeleteAsync(HttpContext httpContext, long id);
        Task<View> UpdateAsync(HttpContext httpContext, Edit model);
    }
}
