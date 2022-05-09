using SportFieldBooking.Biz.Model.Booking;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.Booking
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model, long userId, long sportFieldId);
        Task<Page<List>> GetListAsync(long pageIndex, int pageSize);
    }
}
