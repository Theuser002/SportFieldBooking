using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFieldBooking.Data.Model
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        public long Id { get; set; }
        [StringLength(50)]
        public string Code { get; set; } = "";
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public DateTime BookDate { get; set; }
        public long TotalPrice { get; set; } = 0;
        public virtual User User { get; set; }  
        public virtual SportField SportField { get; set; }
        public virtual BookingStatus? BookingStatus { get; set; }
    }
}
