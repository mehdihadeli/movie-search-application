using FluentValidation;
using MovieSearch.Application.Videos.FindVideos;

namespace Orders.Application.Orders.Features.GetOrderById
{
    public class FindVideosQueryValidator : AbstractValidator<FindVideosQuery>
    {
        public FindVideosQueryValidator()
        {
            RuleFor(query => query.MovieName).NotEmpty().NotNull()
                .WithMessage("movieName should not be null or empty.");
        }
    }
}