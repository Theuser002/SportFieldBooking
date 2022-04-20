using SportFieldBooking.Data.Model;
using SportFieldBooking.Biz.Model.User;

namespace SportFieldBooking.Biz.User
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model);
        Task<View> GetAsync(long id);
        Task<List<List>> GetAllAsync();
        Task<List<List>> GetListAsync(long pageNumber, int pageSize, long total);
    }
}
