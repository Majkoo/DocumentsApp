namespace DocumentsApp.Data.Exceptions;

public class KeyExpiredException : Exception
{
    public KeyExpiredException(string message) : base(message) { }
}