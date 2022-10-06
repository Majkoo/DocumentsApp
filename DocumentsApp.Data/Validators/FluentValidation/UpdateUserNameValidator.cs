using System.Security.Claims;
using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Services;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class UpdateUserNameValidator: AbstractValidator<UpdateUserNameDto>
{
    private readonly IAccountRepo _accountRepo;
    private readonly IAccountContextService _accountContextService;

    public UpdateUserNameValidator(IAccountRepo accountRepo, IAccountContextService accountContextService)
    {
        _accountRepo = accountRepo;
        _accountContextService = accountContextService;

        RuleFor(a => a.NewUserName)
            .NotEmpty()
            .Custom((value, context) =>
            {
                var userName = _accountRepo
                    .GetAccountByEmailAsync(_accountContextService.User.FindFirst(c => c.Type == ClaimTypes.Email)?
                        .Value).Result.UserName;

                if (userName == value)
                {
                    context.AddFailure("UserName", "New username must have different value");
                }
            });

        RuleFor(a => a.Password)
            .NotEmpty();
    }
}