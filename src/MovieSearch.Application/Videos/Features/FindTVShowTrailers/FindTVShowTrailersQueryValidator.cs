using FluentValidation;

namespace MovieSearch.Application.Videos.Features.FindTVShowTrailers
{
    public class FindTVShowTrailersQueryValidator : AbstractValidator<FindTVShowTrailersQuery>
    {
        public FindTVShowTrailersQueryValidator()
        {
            RuleFor(query => query.TVShowId).GreaterThan(0).WithMessage("tvShowId should be greater than zero.");
            RuleFor(query => query.PageSize).GreaterThan(0).WithMessage("page-size should be greater than zero.");
        }
    }
}