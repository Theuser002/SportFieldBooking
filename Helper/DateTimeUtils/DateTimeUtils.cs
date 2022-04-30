namespace SportFieldBooking.Helper.DateTimeUtils
{
    public class DateTimeUtils
    {
        public static TimeSpan ToTimeSpan(string time)
        {
            return DateTime.Parse(DateTime.Parse(time).ToString("HH:mm")).TimeOfDay;
        }

        public static DateTime TakeHourOnly(DateTime time)
        {
            return DateTime.Parse(time.ToString("HH:mm"));
        }
    }
}
