using DocumentsApp.Shared.Dtos.DocumentDtos;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class AddDocumentDtoValidator : AbstractValidator<AddDocumentDto>
{
    public AddDocumentDtoValidator()
    {
        RuleFor(d => d.Name)
            .NotEmpty()
            .MaximumLength(20);

        RuleFor(d => d.Description)
            .MaximumLength(150);

        RuleFor(d => d.Content)
            .MaximumLength(5000)
            .NotEmpty();
    }
}