namespace SportFieldBooking.Biz.Model.JwtAuth
{
    public class LoginResponse
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public int Role { get; set; }

        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime? accessTokenExpiryTime { get; set; }
        public DateTime? refreshTokenExpiryTime { get; set; }
    }
}
