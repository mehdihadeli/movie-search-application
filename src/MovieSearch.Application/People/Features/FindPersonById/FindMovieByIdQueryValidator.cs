using FluentValidation;
using MovieSearch.Application.People.Features.FindPersonById;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class FindPersonByIdQueryValidator : AbstractValidator<FindPersonByIdQuery>
    {
        public FindPersonByIdQueryValidator()
        {
            RuleFor(query => query.PersonId).GreaterThan(0).WithMessage("personId should be greater than zero.");
        }
    }
}