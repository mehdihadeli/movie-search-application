using FluentValidation;
using MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class FindTVShowWithTrailersByIdQueryValidator : AbstractValidator<FindTVShowWithTrailersByIdQuery>
    {
        public FindTVShowWithTrailersByIdQueryValidator()
        {
            RuleFor(query => query.TvShowId).GreaterThan(0).WithMessage("id should be greater than zero.");
            RuleFor(query => query.TrailersCount).GreaterThan(0).WithMessage("trailers-count should be greater than zero.");
        }
    }
}