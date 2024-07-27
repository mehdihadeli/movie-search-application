using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.Videos.Models;
using MovieSearch.Application.Videos.Features.FindMovieTrailers;
using MovieSearch.Application.Videos.Features.FindTrailers;
using MovieSearch.Application.Videos.Features.FindTVShowTrailers;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.Videos;

[ApiVersion("1.0")]
[Route(BaseApiPath + "/[controller]")]
public class VideosController : BaseController
{
    /// <summary>
    ///     Get the trailers by movie or tv-show name
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("search-trailers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Get the trailers by movie or tv-show name",
        Description = "Get the trailers by movie or tv-show name"
    )]
    public async Task<ActionResult> GetTrailersAsync(
        [FromQuery] GetTrailersRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new FindTrailersQuery { MovieName = request.MovieName, PageToken = request.PageToken };
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Get the trailers for a specific movie
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("movie-trailers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get the trailers for a specific movie",
        Description = "Get the trailers for a specific movie"
    )]
    public async Task<ActionResult> GetMovieTrailersAsync(
        [FromQuery] GetMovieTrailersRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new FindMovieTrailersQuery
        {
            MovieId = request.MovieId,
            PageSize = request.PageSize,
            PageToken = request.PageToken
        };
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    ///     Get the trailers for a specific tv-show
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("tvshow-trailers")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [SwaggerOperation(
        Summary = "Get the trailers for a specific tv-show",
        Description = "Get the trailers for a specific tv-show"
    )]
    public async Task<ActionResult> GetTVShowTrailersAsync(
        [FromQuery] GetTVShowTrailersRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new FindTVShowTrailersQuery
        {
            TVShowId = request.TVShowId,
            PageSize = request.PageSize,
            PageToken = request.PageToken
        };
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
