using System.Threading;
using System.Threading.Tasks;
using Asp.Versioning;
using BuildingBlocks.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Api.Multi.Models;
using MovieSearch.Application.Generals.Multi.Features.SearchMultipleModel;
using Swashbuckle.AspNetCore.Annotations;

namespace MovieSearch.Api.Multi;

[ApiVersion("1.0")]
[Route(BaseApiPath + "/[controller]")]
[Authorize]
public class MultiController : BaseController
{
    /// <summary>
    ///     Search multiple models in a single request. Multi search currently supports searching for movies, tv shows and
    ///     people in a single request.
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [SwaggerOperation(
        Summary = "Search multiple models in a single request. Multi search currently supports searching for movies, tv shows and people in a single request.",
        Description = "Search multiple models in a single request. Multi search currently supports searching for movies, tv shows and people in a single request."
    )]
    public async Task<ActionResult> SearchAsync(
        [FromQuery] SearchMultipleModelRequest request,
        CancellationToken cancellationToken
    )
    {
        var query = new SearchMultipleModelQuery
        {
            Page = request.Page,
            Year = request.Year,
            IncludeAdult = request.IncludeAdult,
            SearchKeywords = request.SearchKeywords
        };
        var result = await Mediator.Send(query, cancellationToken);

        return Ok(result);
    }
}
