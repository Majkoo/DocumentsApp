using DocumentsApp.Data.Entities;
using DocumentsApp.Data.Models.AccountModels;
using FluentValidation;

namespace DocumentsApp.Data.Models.FluentValidation;

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
        RuleFor(d => d.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(d => d.ConfirmPassword)
            .Equal(u => u.Password);
    }
}