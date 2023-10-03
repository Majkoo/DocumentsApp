using DocumentsApp.Api.Providers;
using DocumentsApp.Shared.Dtos.Account;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class UpdateUserNameDtoValidator: AbstractValidator<UpdateUserNameDto>
{

    public UpdateUserNameDtoValidator()
    {
        RuleFor(d => d.NewUserName)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(d => d.Password)
            .MinimumLength(8);
    }
}