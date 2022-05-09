﻿namespace SportFieldBooking.Biz.Model.Booking
{
    public class List
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public DateTime BookDate { get; set; }
        public long TotalPrice { get; set; }
    }
}