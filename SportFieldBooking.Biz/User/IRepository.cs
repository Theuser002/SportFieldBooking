using SportFieldBooking.Data.Model;
using SportFieldBooking.Biz.Model.User;

namespace SportFieldBooking.Biz.User
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model);
    }
}
