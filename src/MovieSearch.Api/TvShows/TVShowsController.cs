using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.TvShows.Model;
using MovieSearch.Application.TvShows.Features.FindTvShowById;
using MovieSearch.Application.TvShows.Features.FindTVShowWithTrailersById;
using MovieSearch.Application.TvShows.Features.SearchTVShow;
using MovieSearch.Application.TvShows.Features.SearchTVShowByTitle;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.TvShows
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    public class TVShowsController : BaseController
    {
        /// <summary>
        /// Get specific tv-show by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get specific tv-show by id", Description = "Get specific tv-show by id")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new FindTvShowByIdQuery(id);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get specific tv-show by id with its trailers
        /// </summary>
        /// <param name="id"></param>
        /// <param name="trailersCount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/with-trailers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get specific tv-show by id with its trailers",
            Description = "Get specific tv-show by id with its trailers")]
        public async Task<ActionResult> GetWithTrailersByIdAsync([FromRoute] int id, [FromQuery] int trailersCount = 20,
            CancellationToken cancellationToken = default)
        {
            var query = new FindTVShowWithTrailersByIdQuery(id, trailersCount);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Search tv-shows by title
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-by-title")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Search tv-shows by title", Description = "Search tv-shows by title")]
        public async Task<ActionResult> SearchByTitleAsync([FromQuery] SearchTVShowsByTitleRequest request,
            CancellationToken cancellationToken)
        {
            var query = new SearchTVShowByTitleQuery(request.SearchKeywords, request.Page);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Search tv-shows by different parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Search tv-shows by different parameters",
            Description = "Search tv-shows by different parameters")]
        public async Task<ActionResult> SearchAsync([FromQuery] SearchTVShowsRequest request,
            CancellationToken cancellationToken)
        {
            var query = new SearchTVShowQuery(request.SearchKeywords, request.Page, request.FirstAirDateYear,
                request.IncludeAdult);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}