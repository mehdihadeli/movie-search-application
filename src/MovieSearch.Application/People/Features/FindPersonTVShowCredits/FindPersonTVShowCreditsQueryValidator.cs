using FluentValidation;
using MovieSearch.Application.People.Features.FindPersonTVShowCredits;

namespace Orders.Application.Orders.Features.GetOrderById;

public class FindPersonTVShowCreditsQueryValidator : AbstractValidator<FindPersonTVShowCreditsQuery>
{
    public FindPersonTVShowCreditsQueryValidator()
    {
        RuleFor(query => query.PersonId).GreaterThan(0).WithMessage("personId should be greater than zero.");
    }
}