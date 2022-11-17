using FluentValidation;

namespace MovieSearch.Application.Videos.Features.FindTrailers;

public class FindTrailersQueryValidator : AbstractValidator<FindTrailersQuery>
{
    public FindTrailersQueryValidator()
    {
        RuleFor(query => query.MovieName).NotEmpty().NotNull()
            .WithMessage("movieName should not be null or empty.");
        RuleFor(query => query.PageSize).GreaterThan(0).WithMessage("page-size should be greater than zero.");
    }
}