namespace DocumentsApp.Data.Exceptions.IdentityExceptions;

public class InvalidLoginCredentialsException: Exception
{
    public override string Message { get; }

    public InvalidLoginCredentialsException()
    {
        Message = "Login credentials don't match.";
    }

}