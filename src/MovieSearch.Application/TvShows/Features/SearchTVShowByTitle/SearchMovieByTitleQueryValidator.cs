using FluentValidation;
using MovieSearch.Application.TvShows.Features.SearchTVShowByTitle;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class SearchTVShowByTitleQueryValidator : AbstractValidator<SearchTVShowByTitleQuery>
    {
        public SearchTVShowByTitleQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThan(0).WithMessage("page number should be greater than zero.");
            RuleFor(query => query.SearchKeywords).NotEmpty().NotNull().WithMessage("search value should not be null or empty.");
        }
    }
}