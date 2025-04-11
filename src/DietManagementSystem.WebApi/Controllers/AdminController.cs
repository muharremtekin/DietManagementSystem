using DietManagementSystem.Application.Features.User.Commands.Create;
using DietManagementSystem.Application.Features.User.Commands.Delete;
using DietManagementSystem.Application.Features.User.Commands.Update;
using DietManagementSystem.Application.Features.User.Queries.GetUserById;
using DietManagementSystem.Application.Features.User.Queries.GetUsers;
using DietManagementSystem.Common.Constants;
using DietManagementSystem.WebApi.Controllers.Base;
using DietManagementSystem.WebApi.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;

/// <summary>
/// Controller for managing administrator accounts and operations
/// </summary>
[Authorize(Policy = "AdminPolicy")]
[Route("api/v{version:apiVersion}/admins")]
[ApiController]
[ApiVersion("1.0")]
public class AdminController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the AdminController
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries</param>
    public AdminController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves a specific administrator by their ID
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator</param>
    /// <returns>The administrator details</returns>
    /// <response code="200">Returns the administrator details</response>
    /// <response code="404">If the administrator is not found</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne([FromRoute] Guid userId)
    {
        var result = await _mediator.Send(new GetUserByIdQuery(userId));
        return Ok(result);
    }

    /// <summary>
    /// Retrieves a paginated list of all administrators
    /// </summary>
    /// <param name="query">Query parameters for filtering and pagination</param>
    /// <returns>A paginated list of administrators</returns>
    /// <response code="200">Returns the list of administrators</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
    {
        query.Role = RoleConstants.Admin;
        var result = await _mediator.Send(query);
        Response.AddPaginationHeader(result.MetaData);
        return Ok(result);
    }

    /// <summary>
    /// Creates a new administrator account
    /// </summary>
    /// <param name="command">The administrator details to create</param>
    /// <returns>A 201 Created response</returns>
    /// <response code="201">If the administrator was created successfully</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        command.Role = RoleConstants.Admin;
        await _mediator.Send(command);
        return Created();
    }

    /// <summary>
    /// Updates an existing administrator's details
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator to update</param>
    /// <param name="command">The updated administrator details</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the administrator was updated successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the administrator is not found</response>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateOne([FromRoute] Guid userId, [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes an administrator account
    /// </summary>
    /// <param name="userId">The unique identifier of the administrator to delete</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the administrator was deleted successfully</response>
    /// <response code="404">If the administrator is not found</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command);
        return NoContent();
    }
}

