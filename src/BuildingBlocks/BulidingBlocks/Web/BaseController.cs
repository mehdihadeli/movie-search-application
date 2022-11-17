using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Web;

[ApiController]
public abstract class BaseController : Controller
{
    protected const string BaseApiPath = "api/v{version:apiVersion}";
    private IMapper _mapper;

    private IMediator _mediator;

    protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

    protected IMapper Mapper => _mapper ??= HttpContext.RequestServices.GetService<IMapper>();
}