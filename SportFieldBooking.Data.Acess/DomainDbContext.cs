using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SportFieldBooking.Data.Model;

namespace SportFieldBooking.Data
{
    public class DomainDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public DbSet<User> Users { get; set; }
        public DbSet<SportField> SportFields { get; set; }  
        public DbSet<Booking> Bookings { get; set; }    
        public DbSet<BookingStatus> BookingStatuses { get; set; }   

        public DomainDbContext(IConfiguration configuration)
        {
            Configuration = configuration;  
        }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(e => e.IsActive);
            modelBuilder.Entity<BookingStatus>()
                .HasIndex(e => e.Name);
            modelBuilder.Entity<Booking>()
                .HasIndex(e => e.Code);
            modelBuilder.Entity<SportField>()
                .HasIndex(e => e.PriceHourly);

            modelBuilder.Entity<User>()
                .Property(f => f.Created)
                .HasColumnType("datetime2")
                .HasPrecision(0); // No precision for the fractional second of datetime2 type

            modelBuilder.Entity<SportField>()
                .Property(f => f.RequestOn)
                .HasColumnType("datetime2")
                .HasPrecision(0);
        }

    }
}
