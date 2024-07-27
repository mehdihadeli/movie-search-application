using FluentValidation;
using MovieSearch.Application.TvShows.Features.FindTvShowById;

namespace Orders.Application.Orders.Features.GetOrderById;

public class FindTvShowByIdQueryValidator : AbstractValidator<FindTvShowByIdQuery>
{
    public FindTvShowByIdQueryValidator()
    {
        RuleFor(query => query.TvShowId).GreaterThan(0).WithMessage("id should be greater than zero.");
    }
}
