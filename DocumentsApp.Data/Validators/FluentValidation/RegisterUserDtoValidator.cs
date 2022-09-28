using DocumentsApp.Data.Dtos.AccountDtos;
using DocumentsApp.Data.Entities;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class RegisterUserDtoValidator : AbstractValidator<RegisterUserDto>
{
    public RegisterUserDtoValidator(DocumentsAppDbContext dbContext)
    {
        RuleFor(d=>d.UserName)
            .NotEmpty()
            .MaximumLength(20);
        
        RuleFor(d => d.Email)
            .EmailAddress()
            .Custom((value, context) =>
            {
                if (dbContext.Users.Any(u => u.Email == value))
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