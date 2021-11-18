using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.People.Models;
using MovieSearch.Application.People.Features.FindPersonById;
using MovieSearch.Application.People.Features.FindPersonMovieCredits;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.People
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    public class PeopleController : BaseController
    {
        /// <summary>
        /// Get the primary person details by id.
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
        /// Get the movie credits for a person.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("movie-credits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get the movie credits for a person.",
            Description = "Get the movie credits for a person.")]
        public async Task<ActionResult> GetPersonMovieCreditsAsync([FromQuery] GetPersonMovieCreditsRequest request,
            CancellationToken cancellationToken)
        {
            var query = new FindPersonMovieCreditsQuery(request.PersonId);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }


    }
}