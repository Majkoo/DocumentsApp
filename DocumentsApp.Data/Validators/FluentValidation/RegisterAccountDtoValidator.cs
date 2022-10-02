using DocumentsApp.Data.Entities;
using DocumentsApp.Shared.Dtos.AccountDtos;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class RegisterAccountDtoValidator : AbstractValidator<RegisterAccountDto>
{
    public RegisterAccountDtoValidator(DocumentsAppDbContext dbContext)
    {
        RuleFor(d=>d.UserName)
            .NotEmpty()
            .MaximumLength(20);
        
        RuleFor(d => d.Email)
            .EmailAddress()
            .Custom((value, context) =>
            {
                if (dbContext.Accounts.Any(u => u.Email == value))
                    context.AddFailure("Email", "Email is already taken");
            });

        //TODO 
        //add validation for strong password

        // e tam uzytkownik sam niech dba o haslo
        // chyba ze dla sportu regexika chcesz pisac
        RuleFor(d => d.Password)
            .MinimumLength(8);

        RuleFor(d => d.ConfirmPassword)
            .Equal(u => u.Password);
    }
}