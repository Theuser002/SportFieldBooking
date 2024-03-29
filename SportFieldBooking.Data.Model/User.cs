﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SportFieldBooking.Data.Model
{
    [Table("Users")]
    public class User
    {
        public User()
        {
            Bookings = new HashSet<Booking>();
            // Lay thoi gian hien tai khoi tao cho Created
            Created = DateTime.Now;
        }

        [Key]
        public long Id { get; set; }
        [StringLength(50)] // Datatype constraining
        public string Code { get; set; } = "";
        [StringLength(30)]
        public string? Email { get; set; } = "";
        [StringLength(100)]
        public string Username { get; set; } = "";
        [StringLength(30)]
        public string Password { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public DateTime Created { get; set; }
        public long Balance { get; set; } = 0;
        public int Role { get; set; } = 1; //0:Admin 1:User // Only setable by the system
        public string? RefreshToken { get; set; }
        public DateTime? refreshTokenExpiryTime { get; set; }
        public virtual ICollection<Booking>? Bookings { get; set; }
    }
}