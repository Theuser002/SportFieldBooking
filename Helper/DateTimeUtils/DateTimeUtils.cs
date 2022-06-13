namespace SportFieldBooking.Helper.DateTimeUtils
{
    public class DateTimeUtils
    {
        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Method bien chuoi chi thoi gian ve dang TimeSpan
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(string time)
        {
            return DateTime.Parse(DateTime.Parse(time).ToString("HH:mm")).TimeOfDay;
        }

        /// <summary>
        /// Auth: Hung
        /// Created: 25/04/2022
        /// Method lay ra phan time cua mot DateTime va bien no thanh mot DateTime khac
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime TakeHourOnly(DateTime time)
        {
            return DateTime.Parse(time.ToString("HH:mm"));
        }
    }
}
