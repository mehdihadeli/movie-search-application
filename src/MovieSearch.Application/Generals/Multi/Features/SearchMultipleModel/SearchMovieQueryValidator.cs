using FluentValidation;

namespace MovieSearch.Application.Generals.Multi.Features.SearchMultipleModel;

public class SearchMultipleModelQueryValidator : AbstractValidator<SearchMultipleModelQuery>
{
    public SearchMultipleModelQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThan(0).WithMessage("page number should be greater than zero.");
        RuleFor(query => query.Year)
            .Must(x => x >= 1700)
            .When(c => c.Year > 0)
            .WithMessage("year should be greater than 1700."); //if it's not a default value
        RuleFor(query => query.Year)
            .Must(x => x >= 1700)
            .When(c => c.Year > 0)
            .WithMessage("primary release year should be greater than 1700.");
        RuleFor(query => query.SearchKeywords)
            .NotEmpty()
            .NotNull()
            .WithMessage("search value should not be null or empty.");
    }
}
