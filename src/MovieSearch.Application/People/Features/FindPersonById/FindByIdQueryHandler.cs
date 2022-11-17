using System.Threading;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using AutoMapper;
using MediatR;
using MovieSearch.Application.People.Dtos;
using MovieSearch.Application.People.Exceptions;
using MovieSearch.Application.Services.Clients;

namespace MovieSearch.Application.People.Features.FindPersonById;

public class FindPersonByIdQueryHandler : IRequestHandler<FindPersonByIdQuery, FindPersonByIdQueryResult>
{
    private readonly IMapper _mapper;
    private readonly IMovieDbServiceClient _movieDbServiceClient;

    public FindPersonByIdQueryHandler(IMovieDbServiceClient movieDbServiceClient, IMapper mapper)
    {
        _movieDbServiceClient = movieDbServiceClient;
        _mapper = mapper;
    }

    public async Task<FindPersonByIdQueryResult> Handle(FindPersonByIdQuery query,
        CancellationToken cancellationToken)
    {
        Guard.Against.Null(query, nameof(FindPersonByIdQuery));

        var person = await _movieDbServiceClient.GetPersonDetailAsync(query.PersonId, cancellationToken);

        if (person is null)
            throw new PersonNotFoundException(query.PersonId);

        var result = _mapper.Map<PersonDto>(person);

        return new FindPersonByIdQueryResult {Person = result};
    }
}