// Biz model for creating new User entity
using System.ComponentModel.DataAnnotations;

namespace SportFieldBooking.Biz.Model.User
{
    public class New
    {
        public string Code { get; set; } = "DEFAULT_CODE";
        public string? Email { get; set; } = "";
        [StringLength(100)]
        public string Username { get; set; } = "";
        [StringLength(30)]
        public string Password { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }
}
