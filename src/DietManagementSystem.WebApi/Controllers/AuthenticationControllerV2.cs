using DietManagementSystem.Application.Features.User.Commands.Login;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/authentication")]
[ApiVersion("2.0")]
public class AuthenticationControllerV2 : BaseController
{
    public AuthenticationControllerV2(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Authenticates a user and returns JWT tokens
    /// </summary>
    /// <param name="command">Login credentials</param>
    /// <returns>JWT access token and refresh token</returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
} 