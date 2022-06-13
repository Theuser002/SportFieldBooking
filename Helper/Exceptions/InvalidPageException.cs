using System;

namespace SportFieldBooking.Helper.Exceptions
{
    public class InvalidPageException : Exception
    {
        public InvalidPageException()
        {

        }

        public InvalidPageException(string message) : base(message)
        {

        }
    }
}
