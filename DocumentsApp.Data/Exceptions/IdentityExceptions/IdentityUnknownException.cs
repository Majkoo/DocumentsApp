namespace DocumentsApp.Data.Exceptions.IdentityExceptions;

public class IdentityUnknownException: Exception
{
    public IdentityUnknownException(string message) : base(message) { }
}