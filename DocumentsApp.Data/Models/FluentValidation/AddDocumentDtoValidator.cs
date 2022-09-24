using DocumentsApp.Data.Models.EntityModels.DocumentModels;
using FluentValidation;

namespace DocumentsApp.Data.Models.FluentValidation;

public class AddDocumentDtoValidator : AbstractValidator<AddDocumentDto>
{
    public AddDocumentDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(d => d.Description)
            .MaximumLength(50);

        RuleFor(d => d.Content)
            .NotEmpty();
    }
}