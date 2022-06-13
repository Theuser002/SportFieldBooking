using System.ComponentModel.DataAnnotations;

namespace SportFieldBooking.Biz.Model.JwtAuth
{
    public class RegisterRequest
    {
        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; } = "";
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = "";
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; } = "";
        [Required(ErrorMessage = "Password confirmation is required")]
        public string PasswordConfirmation { get; set; } = "";
    }
}
