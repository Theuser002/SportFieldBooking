﻿namespace SportFieldBooking.Biz.Model.User
{
    public class CurrentUser
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public string? Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsActive { get; set; }
        public int Role { get; set; }
    }
}
