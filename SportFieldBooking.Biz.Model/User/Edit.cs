// Biz model for updating User entities
namespace SportFieldBooking.Biz.Model.User
{
    public class Edit
    {
        public long Id { get; set; }
        public string? Email { get; set; } = "";
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public long Balance { get; set; }
    }
}
