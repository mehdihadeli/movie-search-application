using FluentValidation;
using MovieSearch.Application.Movies.Features.FindMovieByImdbId;

namespace Orders.Application.Orders.Features.GetOrderById;

public class FindMovieByImdbIdQueryValidator : AbstractValidator<FindMovieByImdbIdQuery>
{
    public FindMovieByImdbIdQueryValidator()
    {
        RuleFor(query => query.ImdbId).NotNull().NotEmpty().WithMessage("ImdbId should not be null or empty.");
    }
}