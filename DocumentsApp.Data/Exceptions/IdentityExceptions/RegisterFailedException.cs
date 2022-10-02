using Microsoft.AspNetCore.Identity;

namespace DocumentsApp.Data.Exceptions.IdentityExceptions;

public class RegisterFailedException: Exception
{
    public IEnumerable<IdentityError> IdentityErrors { get; set; }

    public RegisterFailedException(
        IEnumerable<IdentityError> identityErrors,
        string message
        ): base(message)
    {
        IdentityErrors = identityErrors;
    }
}