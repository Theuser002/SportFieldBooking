﻿namespace SportFieldBooking.Biz.Model.JwtAuth
{
    public class RefreshTokenRequest
    {
        public string? AccessToken { get; set; }    
        public string? RefreshToken { get; set; }
    }
}
