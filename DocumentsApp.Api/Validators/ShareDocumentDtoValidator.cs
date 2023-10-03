using DocumentsApp.Shared.Dtos.ShareDocument;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class ShareDocumentDtoValidator : AbstractValidator<ShareDocumentDto>
{
    public ShareDocumentDtoValidator()
    {
        RuleFor(d => d.ShareToNameOrEmail)
            .NotEmpty();
        RuleFor(d => d.AccessLevelEnum)
            .NotNull();
    }
}