using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;


namespace SportFieldBooking.Helper.DateTimeUtils
{
    public class TimeOnlyConverter : ValueConverter<TimeOnly, TimeSpan>
    {
        public TimeOnlyConverter() : base(
                timeOnly => timeOnly.ToTimeSpan(),
                timeSpan => TimeOnly.FromTimeSpan(timeSpan))
        {
        }

        public static TimeSpan ToTimeSpan(string time)
        {
            return DateTime.Parse(DateTime.Parse(time).ToString("HH:mm")).TimeOfDay;
        }
    }

    public class TimeOnlyComparer : ValueComparer<TimeOnly>
    {
        public TimeOnlyComparer() : base(
            (t1, t2) => t1.Ticks == t2.Ticks,
            t => t.GetHashCode())
        {
        }
    }
}
