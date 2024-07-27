using MovieSearch.Application.People.Dtos;

namespace MovieSearch.Application.People.Features.FindPersonTVShowCredits;

public class FindPersonTVShowCreditsQueryResult
{
    public FindPersonTVShowCreditsQueryResult(PersonTVShowCreditDto personTvShowCredit)
    {
        PersonTVShowCredit = personTvShowCredit;
    }

    public PersonTVShowCreditDto PersonTVShowCredit { get; }
}
