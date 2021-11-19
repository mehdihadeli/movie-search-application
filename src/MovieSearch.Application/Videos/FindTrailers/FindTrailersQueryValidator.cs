using FluentValidation;

namespace MovieSearch.Application.Videos.FindTrailers
{
    public class FindTrailersQueryValidator : AbstractValidator<FindTrailersQuery>
    {
        public FindTrailersQueryValidator()
        {
            RuleFor(query => query.MovieName).NotEmpty().NotNull()
                .WithMessage("movieName should not be null or empty.");
        }
    }
}