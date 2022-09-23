namespace DocumentsApp.Data.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message){}
}