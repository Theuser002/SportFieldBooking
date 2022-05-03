// Biz model for listing User entities
namespace SportFieldBooking.Biz.Model.User
{
    public class List
    {
        public long Id { get; set; }
        public string Code { get; set; } = "";
        public string? Email { get; set; } = "";
        public string Username { get; set; } = "";
        public bool IsActive { get; set; } = true;
        public DateTime Created { get; set; }
        public long Balance { get; set; }
    }
}
