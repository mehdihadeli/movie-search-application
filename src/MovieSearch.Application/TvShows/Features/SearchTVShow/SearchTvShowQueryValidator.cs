using FluentValidation;
using MovieSearch.Application.TvShows.Features.SearchTVShow;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class SearchTvShowQueryValidator : AbstractValidator<SearchTVShowQuery>
    {
        public SearchTvShowQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThan(0).WithMessage("page number should be greater than zero.");
            RuleFor(query => query.FirstAirDateYear).Must(x => x >= 1700)
                .When(c => c.FirstAirDateYear > 0) //if it's not a default value
                .WithMessage("primary release year should be greater than 1700.");
            RuleFor(query => query.SearchKeywords).NotEmpty().NotNull()
                .WithMessage("search value should not be null or empty.");
        }
    }
}