namespace SportFieldBooking.Biz.Model.Booking
{
    public class New
    {
        public string Code { get; set; } = "";
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public DateTime BookDate { get; set; }
    }
}
