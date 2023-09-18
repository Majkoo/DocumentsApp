using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos.Auth;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator(IAccountRepo accountRepo)
    {
        RuleFor(d => d.UserName)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(d => d.Email)
            .EmailAddress();

        RuleFor(d => d.Password)
            .MinimumLength(8);

        RuleFor(d => d.ConfirmPassword)
            .Equal(u => u.Password);
    }
}