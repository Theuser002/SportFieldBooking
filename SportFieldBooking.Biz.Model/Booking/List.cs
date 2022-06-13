namespace SportFieldBooking.Biz.Model.Booking
{
    public class List
    {
        public long Id { get; set; }
        public string Code { get; set; }
        public DateTime StartHour { get; set; }
        public DateTime EndHour { get; set; }
        public DateTime BookDate { get; set; }
        public long TotalPrice { get; set; }
        public long UserId { get; set; }
        public string UserUsername { get; set; }
        public long SportFieldId { get; set; }
        public string SportFieldName { get; set; }
        public long BookingStatusId { get; set; }
        public string BookingStatusStatusName { get; set; }
    }
}
