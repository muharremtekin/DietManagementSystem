using DietManagementSystem.Application.Features.User.Commands.Create;
using DietManagementSystem.Application.Features.User.Commands.Delete;
using DietManagementSystem.Application.Features.User.Commands.Update;
using DietManagementSystem.Application.Features.User.Queries.GetUserById;
using DietManagementSystem.Application.Features.User.Queries.GetUsers;
using DietManagementSystem.Common.Constants;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;
[Route(RouteConstants.admin)]
[ApiController]
public class AdminController : BaseController
{
    public AdminController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetOne([FromRoute] Guid userId)
    {
        try
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId));
            return Ok(result);
        }
        catch (Exception)
        {

            throw;
        }

    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetUsersQuery(RoleConstants.Admin));
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        command.Role = RoleConstants.Admin;
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateOne([FromRoute] Guid userId, [FromBody] UpdateUserCommand command)
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
}

