using SportFieldBooking.Biz.Model.Booking;

namespace SportFieldBooking.Biz.Booking
{
    public interface IRepository
    {
        Task<View> CreateAsync(New model, long userId, long sportFieldId);
    }
}
