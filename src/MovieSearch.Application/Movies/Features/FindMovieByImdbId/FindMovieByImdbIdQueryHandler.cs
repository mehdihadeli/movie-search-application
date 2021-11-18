using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.Movies.Dtos;
using MovieSearch.Application.Movies.Exceptions;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.Movies.Features.FindMovieByImdbId
{
    public class FindMovieByImdbIdQueryHandler : IRequestHandler<FindMovieByImdbIdQuery, FindByImdbIdQueryResult>
    {
        private readonly IMovieDbServiceClient _movieDbServiceClient;
        private readonly IMapper _mapper;

        public FindMovieByImdbIdQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
        {
            _movieDbServiceClient = movieDbServiceClient;
            _mapper = mapper;
        }

        public async Task<FindByImdbIdQueryResult> Handle(FindMovieByImdbIdQuery query,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(FindMovieByImdbIdQuery));

            var movie = await _movieDbServiceClient.GetMovieByImdbIdAsync(query.ImdbId, cancellationToken);

            if (movie is null)
                throw new MovieNotFoundException(query.ImdbId);

            var result = _mapper.Map<MovieDto>(movie);

            return new FindByImdbIdQueryResult { Movie = result };
        }
    }
}