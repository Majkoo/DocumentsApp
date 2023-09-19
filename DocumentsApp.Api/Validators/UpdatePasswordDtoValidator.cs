using DocumentsApp.Shared.Dtos.Account;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class UpdatePasswordDtoValidator : AbstractValidator<UpdatePasswordDto>
{
    public UpdatePasswordDtoValidator()
    {
        RuleFor(d => d.CurrentPassword)
            .MinimumLength(8);

        RuleFor(d => d.NewPassword)
            .MinimumLength(8);
        
        RuleFor(d => d.ConfirmNewPassword)
            .Equal(d => d.NewPassword);
    }
}