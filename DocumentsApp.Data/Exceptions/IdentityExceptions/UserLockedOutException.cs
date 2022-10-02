namespace DocumentsApp.Data.Exceptions.IdentityExceptions;

public class UserLockedOutException: Exception
{
    public override string Message { get; }

    public UserLockedOutException()
    {
        Message = "This account is locked out due to security issues. Check your e-mail.";
    }
}