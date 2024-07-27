using FluentValidation;

namespace MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;

public class FindMovieWithTrailersByIdQueryValidator : AbstractValidator<FindMovieWithTrailersByIdQuery>
{
    public FindMovieWithTrailersByIdQueryValidator()
    {
        RuleFor(query => query.MovieId).GreaterThan(0).WithMessage("id should be greater than zero.");
        RuleFor(query => query.TrailersCount).GreaterThan(0).WithMessage("trailers-count should be greater than zero.");
    }
}
