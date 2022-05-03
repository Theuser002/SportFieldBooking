using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportFieldBooking.Biz
{
    public interface IRepositoryWrapper
    {
        public User.IRepository User { get; }
        public SportField.IRepository SportField { get; }
        public Booking.IRepository Booking { get; }
        public BookingStatus.IRepository BookingStatus { get; }
    }
}
