using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFieldBooking.Data.Model
{
    [Table("Bookings")]
    public class Booking
    {
        [Key]
        public long Id { get; set; }
        public string Code { get; set; } = "";
        public int StartHour { get; set; } 
        public int EndHour { get; set; }
        public int BookDate { get; set; }
        public virtual User User { get; set; }  
        public virtual SportField SportField { get; set; }  
        public virtual BookingStatus BookingStatus { get; set; }
    }
}
