using DietManagementSystem.Application.Features.User.Commands.Login;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;

/// <summary>
/// Controller for handling user authentication operations
/// </summary>
[ApiController]
[Route("api/v{version:apiVersion}/authentication")]
[ApiVersion("1.0")]
public class AuthenticationController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the AuthenticationController
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries</param>
    public AuthenticationController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token
    /// </summary>
    /// <param name="command">The login credentials</param>
    /// <returns>A JWT token if authentication is successful</returns>
    /// <response code="200">Returns the JWT token</response>
    /// <response code="400">If the login credentials are invalid</response>
    /// <response code="401">If the user is not found or password is incorrect</response>
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

