namespace DocumentsApp.Data.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string message) : base(message) {}
}