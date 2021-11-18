using FluentValidation;
using MovieSearch.Application.People.Features.FindPersonMovieCredits;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class FindPersonMovieCreditsQueryValidator : AbstractValidator<FindPersonMovieCreditsQuery>
    {
        public FindPersonMovieCreditsQueryValidator()
        {
            RuleFor(query => query.PersonId).GreaterThan(0).WithMessage("personId should be greater than zero.");
        }
    }
}