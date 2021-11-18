using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.People.Dtos;
using MovieSearch.Application.People.Exceptions;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.People.Features.FindPersonMovieCredits
{
    public class
        FindPersonMovieCreditsQueryHandler : IRequestHandler<FindPersonMovieCreditsQuery,
            FindPersonMovieCreditsQueryResult>
    {
        private readonly IMovieDbServiceClient _movieDbServiceClient;
        private readonly IMapper _mapper;

        public FindPersonMovieCreditsQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
        {
            _movieDbServiceClient = movieDbServiceClient;
            _mapper = mapper;
        }

        public async Task<FindPersonMovieCreditsQueryResult> Handle(FindPersonMovieCreditsQuery query,
            CancellationToken cancellationToken)
        {
            Guard.Against.Null(query, nameof(FindPersonMovieCreditsQuery));

            var personMovieCredit = await _movieDbServiceClient.GetPersonMovieCreditsAsync(query.PersonId, cancellationToken);

            if (personMovieCredit is null)
                throw new PersonMovieCreditsNotFoundException(query.PersonId);

            var result = _mapper.Map<PersonMovieCreditDto>(personMovieCredit);

            return new FindPersonMovieCreditsQueryResult(result);
        }
    }
}