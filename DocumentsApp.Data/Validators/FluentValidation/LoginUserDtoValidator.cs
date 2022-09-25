using DocumentsApp.Data.Dtos.EntityModels.AccountModels;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(d => d.Email)
            .EmailAddress();

        RuleFor(d => d.Password)
            .NotEmpty();
    }
}