using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.Videos.Models;
using MovieSearch.Application.Videos.FindTrailers;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.Videos
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    public class VideosController : BaseController
    {
        /// <summary>
        /// Get the trailers by movie or tv-show name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-trailers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Get the trailers by movie or tv-show name", Description = "Get the trailers by movie or tv-show name")]
        public async Task<ActionResult> GetTrailersAsync([FromQuery] GetTrailersRequest request,
            CancellationToken cancellationToken)
        {
            var query = new FindTrailersQuery
            {
                MovieName = request.MovieName,
                PageToken = request.PageToken
            };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}