using FluentValidation;
using MovieSearch.Application.Movies.Features.FindPopularMovies;

namespace MovieSearch.Application.Movies.Features.SearchMovie;

public class FindPopularMoviesQueryValidator : AbstractValidator<FindPopularMoviesQuery>
{
    public FindPopularMoviesQueryValidator()
    {
        RuleFor(query => query.Page).GreaterThan(0).WithMessage("page number should be greater than zero.");
    }
}