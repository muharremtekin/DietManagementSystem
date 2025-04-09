using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DietManagementSystem.WebApi.Controllers.Base;

[ApiController]
public abstract class BaseController : ControllerBase
{
    internal Guid? UserId
    {
        get
        {
            var val = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return val is null ? null : new Guid(val);
        }
    }

    internal string? UserRole
    {
        get
        {
            var val = HttpContext.User.FindFirst(ClaimTypes.Role)?.Value;
            return val;
        }
    }

    public readonly IMediator _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
}

