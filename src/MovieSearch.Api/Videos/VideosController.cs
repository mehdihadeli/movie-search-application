using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.Videos.Models;
using MovieSearch.Application.Videos.FindVideos;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.Videos
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    public class VideosController : BaseController
    {
        /// <summary>
        /// Get the videos by movie or tv-show name
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Get the videos by movie or tv-show name", Description = "Get the videos by movie or tv-show name")]
        public async Task<ActionResult> GetVideosAsync([FromQuery] GetVideosRequest request,
            CancellationToken cancellationToken)
        {
            var query = new FindVideosQuery
            {
                MovieName = request.MovieName,
                PageToken = request.PageToken
            };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}