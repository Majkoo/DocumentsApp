using DocumentsApp.Data.Dtos.AccountDtos;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class LoginUserDtoValidator : AbstractValidator<LoginUserDto>
{
    public LoginUserDtoValidator()
    {
        RuleFor(d => d.Email)
            .EmailAddress()
            .NotEmpty();

        RuleFor(d => d.Password)
            .NotEmpty();
    }
}