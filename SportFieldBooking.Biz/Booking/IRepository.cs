using Microsoft.AspNetCore.Http;
using SportFieldBooking.Biz.Model.Booking;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.Booking
{
    public interface IRepository
    {
        Task<View> CreateAsync(HttpContext httpContext, New model);
        Task<Page<List>> GetListAsync(HttpContext httpContext, long pageIndex, int pageSize);
        Task DeleteAsync(HttpContext httpContext, long id);
        Task<Page<List>> GetUserBooking(HttpContext httpContext, long userId, long pageIndex, int pageSize);
        Task<Page<List>> GetSportFieldBooking(HttpContext httpContext, long sportFieldId, long pageIndex, int pageSize);
    }
}
