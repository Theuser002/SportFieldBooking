namespace SportFieldBooking.Biz.Model.SportField
{
    public class New
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public double PriceHourly { get; set; }
        public DateTime OpeningHour { get; set; }
        public DateTime ClosingHour { get; set; }
    }
}
