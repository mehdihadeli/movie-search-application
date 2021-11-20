using FluentValidation;

namespace MovieSearch.Application.Videos.FindMovieTrailers
{
    public class FindMovieTrailersQueryValidator : AbstractValidator<FindMovieTrailersQuery>
    {
        public FindMovieTrailersQueryValidator()
        {
            RuleFor(query => query.MovieId).GreaterThan(0).WithMessage("movieId should be greater than zero.");
            RuleFor(query => query.PageSize).GreaterThan(0).WithMessage("page-size should be greater than zero.");
        }
    }
}