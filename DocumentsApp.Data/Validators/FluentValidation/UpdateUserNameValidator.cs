using System.Security.Claims;
using DocumentsApp.Data.Auth;
using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Data.Services;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class UpdateUserNameValidator: AbstractValidator<UpdateUserNameDto>
{

    public UpdateUserNameValidator(CustomAuthenticationStateProvider authenticationStateProvider)
    {
        RuleFor(a => a.NewUserName)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var userName = authenticationStateProvider.GetUserName().Result;

                if (userName == value)
                {
                    context.AddFailure("UserName", "New username must have different value");
                }
            });

        RuleFor(a => a.Password)
            .NotEmpty();
    }
}