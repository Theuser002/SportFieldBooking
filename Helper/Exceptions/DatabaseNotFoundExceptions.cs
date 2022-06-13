using System;

namespace SportFieldBooking.Helper.Exceptions
{
    public class DatabaseNotFoundExceptions : Exception
    {
        public DatabaseNotFoundExceptions()
        {

        }

        public DatabaseNotFoundExceptions(string message) : base(message)
        {

        }
    }
}
