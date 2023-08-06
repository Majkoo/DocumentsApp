using DocumentsApp.Data.Auth.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class UpdateUserNameValidator: AbstractValidator<UpdateUserNameDto>
{

    public UpdateUserNameValidator(IAuthenticationContextProvider authenticationStateProvider)
    {
        RuleFor(a => a.NewUserName)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var userName = authenticationStateProvider.GetUserName();

                if (userName == value)
                {
                    context.AddFailure("UserName", "New username must be different from the current one");
                }
            });

        RuleFor(a => a.Password)
            .NotEmpty();
    }
}