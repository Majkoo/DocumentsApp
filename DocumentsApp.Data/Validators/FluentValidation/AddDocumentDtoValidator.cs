using DocumentsApp.Data.Dtos.EntityModels.DocumentModels;
using FluentValidation;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class AddDocumentDtoValidator : AbstractValidator<AddDocumentDto>
{
    public AddDocumentDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(d => d.Description)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(d => d.Content)
            .NotEmpty();
    }
}