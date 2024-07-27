using MovieSearch.Application.People.Dtos;

namespace MovieSearch.Application.People.Features.FindPersonMovieCredits;

public class FindPersonMovieCreditsQueryResult
{
    public FindPersonMovieCreditsQueryResult(PersonMovieCreditDto personMovieCredit)
    {
        PersonMovieCredit = personMovieCredit;
    }

    public PersonMovieCreditDto PersonMovieCredit { get; }
}
