using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.People.Dtos;
using MovieSearch.Application.People.Exceptions;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.People.Features.FindPersonTVShowCredits;

public class
    FindPersonTVShowCreditsQueryHandler : IRequestHandler<FindPersonTVShowCreditsQuery,
        FindPersonTVShowCreditsQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindPersonTVShowCreditsQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindPersonTVShowCreditsQueryResult> Handle(FindPersonTVShowCreditsQuery query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindPersonTVShowCreditsQuery));

        var personTVShowCredit = await _movieDbServiceClient.GetPersonTvShowCreditsAsync(query.PersonId,
            cancellationToken);

        if (personTVShowCredit is null)
            throw new PersonTVShowCreditsNotFoundException(query.PersonId);

        var result = _mapper.Map<PersonTVShowCreditDto>(personTVShowCredit);

        return new FindPersonTVShowCreditsQueryResult(result);
    }
}