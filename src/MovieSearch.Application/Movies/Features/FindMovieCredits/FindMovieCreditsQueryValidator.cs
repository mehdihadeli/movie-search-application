using FluentValidation;
using MovieSearch.Application.Movies.Features.FindMovieCredits;

namespace MovieSearch.Application.Movies.Features.SearchMovie;

public class FindMovieCreditsQueryValidator : AbstractValidator<FindMovieCreditsQuery>
{
    public FindMovieCreditsQueryValidator()
    {
        RuleFor(query => query.MovieId).GreaterThan(0).WithMessage("movieId should be greater than zero.");
    }
}