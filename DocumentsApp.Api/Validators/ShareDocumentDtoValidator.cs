using DocumentsApp.Shared.Dtos.ShareDocument;
using FluentValidation;

namespace DocumentsApp.Api.Validators;

public class ShareDocumentDtoValidator : AbstractValidator<ShareDocumentDto>
{
    public ShareDocumentDtoValidator()
    {
        RuleFor(s => s.ShareToNameOrEmail)
            .NotEmpty();
        RuleFor(s => s.AccessLevelEnum)
            .NotNull();
    }
}