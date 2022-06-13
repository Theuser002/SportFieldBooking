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
        [StringLength(50)]
        public string Code { get; set; } = "";
        [StringLength(30)]
        public string Name { get; set; } = "";
        public string? Description { get; set; } = "";
        public long PriceHourly { get; set; } = 0;
        [Column(TypeName = "datetime2")]
        public DateTime OpeningHour { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime ClosingHour { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}




