using DocumentsApp.Data.Entities;
using FluentValidation;
using Sieve.Models;

namespace DocumentsApp.Data.Validators.FluentValidation;

public class SieveModelValidator : AbstractValidator<SieveModel>
{
    private readonly int[] _allowedPageSizes = { 5, 10, 15 };

    private readonly string[] _allowedSortValues = 
        { nameof(Document.Name), nameof(Document.DateCreated), nameof(Document.Account.UserName) };
    
    public SieveModelValidator()
    {
        RuleFor(s => s.PageSize)
            .Custom((value, context) =>
            {
                if (!_allowedPageSizes.Contains(value.GetValueOrDefault()))
                {
                    context.AddFailure("PageSize", $"Page Size must be in [{string.Join(",", _allowedPageSizes)}]");
                }
            });
        
        RuleFor(s => s.Page)
            .GreaterThanOrEqualTo(1);
        
        RuleFor(s=>s.Sorts)
            .Must(value => string.IsNullOrEmpty(value) || _allowedSortValues.Contains(value) ||
                           _allowedSortValues.Contains(value.Remove(0,1)))
            .WithMessage($"Sorts is optional or must be in [{string.Join(",", _allowedSortValues)}]");
    }
}