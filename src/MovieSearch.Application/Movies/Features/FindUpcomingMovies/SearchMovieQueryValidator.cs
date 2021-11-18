using FluentValidation;
using MovieSearch.Application.Movies.Features.FindUpcomingMovies;

namespace MovieSearch.Application.Movies.Features.SearchMovie
{
    public class FindUpcomingMoviesQueryQueryValidator : AbstractValidator<FindUpcomingMoviesQuery>
    {
        public FindUpcomingMoviesQueryQueryValidator()
        {
            RuleFor(query => query.Page).GreaterThan(0).WithMessage("page number should be greater than zero.");
        }
    }
}