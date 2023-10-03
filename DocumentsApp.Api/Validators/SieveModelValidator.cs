using DocumentsApp.Data.Entities;
using FluentValidation;
using Sieve.Models;

namespace DocumentsApp.Api.Validators;

public class SieveModelValidator : AbstractValidator<SieveModel>
{
    private readonly string[] _allowedSortValues =
        { nameof(Document.Name), nameof(Document.DateCreated), nameof(Document.Account.UserName) };

    public SieveModelValidator()
    {
        RuleFor(d => d.PageSize)
            .NotNull()
            .LessThanOrEqualTo(500);

        RuleFor(d => d.Page)
            .NotNull()
            .GreaterThanOrEqualTo(1);

        RuleFor(d => d.Sorts)
            .Must(value => string.IsNullOrEmpty(value) || _allowedSortValues.Contains(value) ||
                           _allowedSortValues.Contains(value.Remove(0, 1)))
            .WithMessage($"Sorts must be empty or in (-)[{string.Join(",", _allowedSortValues)}]");
    }
}