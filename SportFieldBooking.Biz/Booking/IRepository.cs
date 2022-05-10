using SportFieldBooking.Biz.Model.Booking;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.Booking
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model);
        Task<Page<List>> GetListAsync(long pageIndex, int pageSize);
        Task DeleteAsync(long id);
        Task<Page<List>> GetUserBooking(long userId, long pageIndex, int pageSize);
        Task<Page<List>> GetSportFieldBooking(long sportFieldId, long pageIndex, int pageSize);
    }
}
