using SportFieldBooking.Biz.Model.SportField;
using SportFieldBooking.Helper.Pagination;

namespace SportFieldBooking.Biz.SportField
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model);
        Task<View> GetAsync(long id);
        Task<Page<List>> GetListAsync(long pageIndex, int pageSize);
        Task DeleteAsync(long id);
        Task<View> UpdateAsync(Edit model);
        Task<Page<List>> SearchNameAsync(string name, long pageIndex, int pageSize);
        Task<Page<List>> FindOpeningAsync(long pageIndex, int pageSize);
        Task<Page<List>> FilterByTime(DateTime timeStart, DateTime timeEnd, long pageIndex, int pageSize);
    }
}
