using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Application.People.Features.FindPersonById;
using MovieSearch.Application.People.Features.FindPersonMovieCredits;
using MovieSearch.Application.People.Features.FindPersonTVShowCredits;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.People;

[ApiVersion("1.0")]
[Route(BaseApiPath + "/[controller]")]
[Authorize]
public class PeopleController : BaseController
{
    /// <summary>
    ///     Get the primary person details by id.
    /// </summary>
    /// <param name="personId"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{personId:int}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get the primary person details by id.",
        Description = "Get the primary person details by id.")]
    public async Task<ActionResult> GetByIdAsync([FromRoute] int personId, CancellationToken cancellationToken)
    {
        var query = new FindPersonByIdQuery(personId);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Get the movie credits for a person.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/movie-credits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get the movie credits for a person.",
        Description = "Get the movie credits for a person.")]
    public async Task<ActionResult> GetPersonMovieCreditsAsync([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new FindPersonMovieCreditsQuery(id);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Get the tv-show credits for a person.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("{id:int}/tvshow-credits")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(Summary = "Get the tv-show credits for a person.",
        Description = "Get the tv-show credits for a person.")]
    public async Task<ActionResult> GetPersonTvShowCreditsAsync([FromRoute] int id,
        CancellationToken cancellationToken)
    {
        var query = new FindPersonTVShowCreditsQuery(id);
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}