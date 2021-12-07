using System.Threading;
using System.Threading.Tasks;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Application.Movies.Features.FindById;
using MovieSearch.Application.Movies.Features.FindMovieByImdbId;
using MovieSearch.Application.Movies.Features.FindMovieCredits;
using MovieSearch.Application.Movies.Features.FindMovieWithTrailersById;
using MovieSearch.Application.Movies.Features.FindMovieWithTrailersByImdbId;
using MovieSearch.Application.Movies.Features.FindPopularMovies;
using MovieSearch.Application.Movies.Features.FindUpcomingMovies;
using MovieSearch.Application.Movies.Features.SearchMovie;
using MovieSearch.Application.Movies.Features.SearchMovieByTitle;
using Swashbuckle.AspNetCore.Annotations;
using Thesaurus.Api.Words.ViewModels;

namespace MovieSearch.Api.Movies
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    [ApiExplorerSettings(GroupName = "Movie Endpoints")]
    [Authorize]
    public class MoviesController : BaseController
    {
        /// <summary>
        /// Get specific movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get specific movie by id", Description = "Get a single movie by Id")]
        public async Task<ActionResult> GetByIdAsync([FromRoute] int id, CancellationToken cancellationToken)
        {
            var query = new FindMovieByIdQuery { Id = id };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get specific movie by imdbId
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{imdbId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get specific order by id", Description = "Get a single Order by Id")]
        public async Task<ActionResult> GetByImdbIdAsync([FromRoute] string imdbId, CancellationToken cancellationToken)
        {
            var query = new FindMovieByImdbIdQuery { ImdbId = imdbId };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get specific movie by id and its trailers
        /// </summary>
        /// <param name="trailersCount"></param>
        /// <param name="cancellationToken"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id:int}/with-trailers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get specific movie by id with its trailers",
            Description = "Get specific movie by id with its trailers")]
        public async Task<ActionResult> GetWithTrailersById([FromRoute] int id, [FromQuery] int trailersCount = 20,
            CancellationToken cancellationToken = default)
        {
            var query = new FindMovieWithTrailersByIdQuery(id, trailersCount);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get specific movie by imdbId with its trailers.
        /// </summary>
        /// <param name="imdbId"></param>
        /// <param name="trailersCount"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{imdbId}/with-trailers")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get specific movie by imdbId with its trailers.",
            Description = "Get specific movie by imdbId with its trailers.")]
        public async Task<ActionResult> GetWithTrailersByImdbId([FromRoute] string imdbId,
            [FromQuery] int trailersCount = 20, CancellationToken cancellationToken = default)
        {
            var query = new FindMovieWithTrailersByImdbIdQuery(imdbId, trailersCount);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Search movies by title
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search-by-title")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Search movies by title", Description = "Search movies by title")]
        public async Task<ActionResult> SearchByTitleAsync([FromQuery] SearchMoviesByTitleRequest request,
            CancellationToken cancellationToken)
        {
            var query = new SearchMovieByTitleQuery
            {
                Page = request.Page, SearchKeywords = request.SearchKeywords
            };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Search movies by different parameters
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Search movies by different parameters",
            Description = "Search movies by different parameters")]
        public async Task<ActionResult> SearchAsync([FromQuery] SearchMoviesRequest request,
            CancellationToken cancellationToken)
        {
            var query = new SearchMovieQuery(request.SearchKeywords, request.Page, request.Year, request
                .PrimaryReleaseYear, request.IncludeAdult);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get a list of upcoming movies in theatres
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("upcoming")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Get a list of upcoming movies in theatres",
            Description = "Get a list of upcoming movies in theatres")]
        public async Task<ActionResult> GetUpcomingMoviesAsync([FromQuery] GetUpcomingMoviesRequest request,
            CancellationToken cancellationToken)
        {
            var query = new FindUpcomingMoviesQuery { Page = request.Page };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get a list of the current popular movies on TMDB. This list updates daily.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("popular")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [SwaggerOperation(Summary = "Get a list of the current popular movies on TMDB. This list updates daily.",
            Description = "Get a list of the current popular movies on TMDB. This list updates daily.")]
        public async Task<ActionResult> GetPopularMoviesAsync([FromQuery] GetPopularMoviesRequest request,
            CancellationToken cancellationToken)
        {
            var query = new FindPopularMoviesQuery { Page = request.Page };
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }

        /// <summary>
        /// Get the cast and crew for a movie.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("credits")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerOperation(Summary = "Get the cast and crew for a movie.",
            Description = "Get the cast and crew for a movie.")]
        public async Task<ActionResult> GetMovieCreditsAsync([FromQuery] GetMovieCreditsRequest request,
            CancellationToken cancellationToken)
        {
            var query = new FindMovieCreditsQuery(request.MovieId);
            var result = await Mediator.Send(query, cancellationToken);

            return Ok(result);
        }
    }
}