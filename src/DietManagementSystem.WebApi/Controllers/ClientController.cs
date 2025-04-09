using DietManagementSystem.Application.Features.User.Commands.Create;
using DietManagementSystem.Application.Features.User.Commands.Delete;
using DietManagementSystem.Application.Features.User.Commands.Update;
using DietManagementSystem.Application.Features.User.Queries.GetUserById;
using DietManagementSystem.Application.Features.User.Queries.GetUsers;
using DietManagementSystem.Common.Constants;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;

[Authorize(Policy = "DietitianPolicy")]
[Route(RouteConstants.client)]
[ApiController]
public class ClientController : BaseController
{
    public ClientController(IMediator mediator) : base(mediator)
    {
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        command.Role = RoleConstants.Client;
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetOne([FromRoute] Guid userId)
    {
        var client = await _mediator.Send(new GetUserByIdQuery(userId));

        return Ok(client);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var clients = await _mediator.Send(new GetUsersQuery(RoleConstants.Client));
        return Ok(clients);
    }
}