using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using SportFieldBooking.Data.Model;
using SportFieldBooking.Helper.DateTimeUtils;

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
                .HasIndex(e => e.Code)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Email)
                .IsUnique();
            modelBuilder.Entity<User>()
                .HasIndex(e => e.Username)
                .IsUnique();
            modelBuilder.Entity<Booking>()
                .HasIndex(e => e.Code)
                .IsUnique();
            modelBuilder.Entity<SportField>()
                .HasIndex(e => e.Code)
                .IsUnique();
            modelBuilder.Entity<BookingStatus>()
                .HasIndex(e => e.StatusName)
                .IsUnique();

            modelBuilder.Entity<User>()
                .Property(f => f.Created)
                .HasColumnType("datetime2")
                .HasPrecision(0); // No precision for the fractional second of datetime2 type
            modelBuilder.Entity<Booking>()
                .Property(f => f.BookDate)
                .HasColumnType("datetime2")
                .HasPrecision(0);
            modelBuilder.Entity<SportField>()
                .Property(f => f.OpeningHour)
                .HasColumnType("datetime2")
                .HasPrecision(0);
            modelBuilder.Entity<SportField>()
                .Property(f => f.ClosingHour)
                .HasColumnType("datetime2")
                .HasPrecision(0);
        }
    }
}