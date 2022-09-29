using DocumentsApp.Data.Dtos.AccountDtos;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class LoginAccountDtoValidator : AbstractValidator<LoginAccountDto>
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