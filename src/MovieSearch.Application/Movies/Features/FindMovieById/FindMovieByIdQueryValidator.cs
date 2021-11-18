using FluentValidation;
using MovieSearch.Application.Movies.Features.FindById;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class FindMovieByIdQueryValidator : AbstractValidator<FindMovieByIdQuery>
    {
        public FindMovieByIdQueryValidator()
        {
            RuleFor(query => query.Id).GreaterThan(0).WithMessage("id should be greater than zero.");
        }
    }
}