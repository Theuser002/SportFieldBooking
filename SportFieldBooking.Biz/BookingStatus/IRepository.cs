using SportFieldBooking.Biz.Model.BookingStatus;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.BookingStatus
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model);
        Task<View> GetAsync(long id);
        Task<Page<List>> GetListsAsync(long pageIndex, int pageSize);
        Task DeleteAsync(long id);
        Task<View> UpdateAsync(Edit model);
    }
}
