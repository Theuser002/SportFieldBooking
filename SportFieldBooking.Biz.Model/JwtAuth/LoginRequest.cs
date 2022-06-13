using System.ComponentModel.DataAnnotations;

namespace SportFieldBooking.Biz.Model.JwtAuth
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = "";
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
    }
}
