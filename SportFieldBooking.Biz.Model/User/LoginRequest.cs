using System.ComponentModel.DataAnnotations;

namespace SportFieldBooking.Biz.Model.User
{
    public class LoginRequest
    {
        [EmailAddress]
        [Required(ErrorMessage = "Username is required")]
        public string Email { get; set; } = "";
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
    }
}
