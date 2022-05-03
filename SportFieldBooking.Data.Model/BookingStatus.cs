using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFieldBooking.Data.Model
{
    [Table("BookingStatuses")]
    public class BookingStatus
    {
        public BookingStatus()
        {
            Bookings = new HashSet<Booking>();  
        }
        [Key]
        public long Id { get; set; }
        public string Status { get; set; } = "";
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
