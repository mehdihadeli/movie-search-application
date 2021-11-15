using AutoMapper;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;

namespace BuildingBlocks.Web
{
    [ApiController]
    public abstract class BaseController : Controller
    {
        protected const string BaseApiPath = "api/v{version:apiVersion}";

        private IMediator _mediator;
        private IMapper _mapper;

        protected IMediator Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
    }
}