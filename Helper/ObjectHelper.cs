using Newtonsoft.Json;

namespace SportFieldBooking.Helper
{
    public static class ObjectHelper
    {
        public static void Dump<T>(this T x)
        {
            string json = JsonConvert.SerializeObject(x, Formatting.Indented);
            Console.WriteLine(json);
        }
    }
}
