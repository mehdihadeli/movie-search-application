using FluentValidation;
using MovieSearch.Application.Movies.Features.FindMovieWithTrailersByImdbId;

namespace Orders.Application.Orders.Features.GetOrderById;

public class FindMovieWithTrailersByImdbIdQueryValidator : AbstractValidator<FindMovieWithTrailersByImdbIdQuery>
{
    public FindMovieWithTrailersByImdbIdQueryValidator()
    {
        RuleFor(query => query.ImdbId).NotNull().NotEmpty().WithMessage("ImdbId should not be null or empty.");
        RuleFor(query => query.TrailersCount)
            .NotNull()
            .NotEmpty()
            .WithMessage("trailers-count should not be null or empty.");
    }
}
