using DocumentsApp.Shared.Dtos.Auth;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(d => d.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(d => d.Password)
            .NotEmpty();
    }
}