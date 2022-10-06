using DocumentsApp.Data.Repos;
using DocumentsApp.Data.Repos.Interfaces;
using DocumentsApp.Shared.Dtos.AccountDtos;
using FluentValidation;
using Sieve.Extensions;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class RegisterAccountDtoValidator : AbstractValidator<RegisterAccountDto>
{
    public RegisterAccountDtoValidator(IAccountRepo accountRepo)
    {
        RuleFor(d=>d.UserName)
            .NotEmpty()
            .MaximumLength(20)
            .Custom((value, context) =>
            {
                var accounts =  accountRepo.GetAllAccountsAsync().Result;
                if (accounts.Any(a => a.UserName == value))
                {
                    context.AddFailure("UserName", "UserName is already taken");
                }
            });
        
        RuleFor(d => d.Email)
            .EmailAddress()
            .Custom((value, context) =>
            {
                var accounts =  accountRepo.GetAllAccountsAsync().Result;
                if (accounts.Any(a => a.Email == value))
                {
                    context.AddFailure("Email", "Email is already taken");
                }
            });

        RuleFor(d => d.Password)
            .MinimumLength(8);

        RuleFor(d => d.ConfirmPassword)
            .Equal(u => u.Password);
    }
}