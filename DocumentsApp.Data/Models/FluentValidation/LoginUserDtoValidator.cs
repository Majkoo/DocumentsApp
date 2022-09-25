using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Models.AccountModels;
using FluentValidation;

namespace DocumentsApp.Data.Models.FluentValidation;

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