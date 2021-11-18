using BuildingBlocks.Web;
using Microsoft.AspNetCore.Mvc;

namespace MovieSearch.Api.Multi
{
    [ApiVersion("1.0")]
    [Route(BaseApiPath + "/[controller]")]
    public class MultiController : BaseController
    {
    }
}