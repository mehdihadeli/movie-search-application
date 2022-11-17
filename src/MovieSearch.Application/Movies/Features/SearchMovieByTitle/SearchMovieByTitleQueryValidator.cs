using FluentValidation;
using MovieSearch.Application.Movies.Features.SearchMovieByTitle;

namespace Orders.Application.Orders.Features.GetOrderById;

public class SearchMovieByTitleQueryValidator : AbstractValidator<SearchMovieByTitleQuery>
{
    public SearchMovieByTitleQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThan(0).WithMessage("page number should be greater than zero.");
        RuleFor(query => query.SearchKeywords).NotEmpty().NotNull()
            .WithMessage("search value should not be null or empty.");
    }
}