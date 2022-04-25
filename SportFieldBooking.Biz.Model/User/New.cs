// Biz model for creating new User entity
namespace SportFieldBooking.Biz.Model.User
{
    public class New
    {
        public string Code { get; set; } = "DEFAULT_CODE";
        public string? Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public bool IsActive { get; set; } = true;
    }
}
