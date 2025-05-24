namespace BookingSystem.Core.Exceptions;

public class BookingException : Exception
{
    public BookingException(string message) : base(message)
    {
    }
}