using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Application.Movies.Features.FindById;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.Movies
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    public class MoviesController: BaseController
    {
        /// <summary>
        /// Gets specific movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}", Name = "GetByIdAsync")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Gets specific order by id", Description = "Gets a single Order by Id")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new FindMovieByIdQuery { Id = id };
            FindMovieByIdQueryResult result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}