using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFieldBooking.Data.Model
{
    [Table("SportFields")]
    public class SportField
    {
        public SportField()
        {
            Bookings = new HashSet<Booking>();
        }
        [Key]
        public long Id { get; set; }
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public double PriceHourly { get; set; } = 0.0;
        public int OpeningHour { get; set; }
        public int ClosingHour { get; set; }
        public DateTime RequestOn { get; set; }

        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}
