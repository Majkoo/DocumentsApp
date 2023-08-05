using DocumentsApp.Shared.Dtos.AccountDtos;
using DocumentsApp.Shared.Dtos.Auth;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class LoginAccountDtoValidator : AbstractValidator<LoginDto>
{
    public LoginAccountDtoValidator()
    {
        RuleFor(d => d.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(d => d.Password)
            .NotEmpty();
    }
}