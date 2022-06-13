namespace SportFieldBooking.Biz.Model.JwtAuth
{
    public class UserWithToken
    {
        public string Username { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? refreshTokenExpiryTime { get; set; }
    }
}
