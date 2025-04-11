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
/// Controller for managing client accounts and operations
/// </summary>
[Authorize(Policy = "DietitianPolicy")]
[Route("api/v{version:apiVersion}/clients")]
[ApiController]
[ApiVersion("1.0")]
public class ClientController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the ClientController
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries</param>
    public ClientController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates a new client account
    /// </summary>
    /// <param name="command">The client details to create</param>
    /// <returns>A 201 Created response</returns>
    /// <response code="201">If the client was created successfully</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
    {
        command.Role = RoleConstants.Client;
        await _mediator.Send(command);
        return Created();
    }

    /// <summary>
    /// Updates an existing client's details
    /// </summary>
    /// <param name="userId">The unique identifier of the client to update</param>
    /// <param name="command">The updated client details</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the client was updated successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the client is not found</response>
    [HttpPut("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid userId, [FromBody] UpdateUserCommand command)
    {
        command.UserId = userId;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a client account
    /// </summary>
    /// <param name="userId">The unique identifier of the client to delete</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the client was deleted successfully</response>
    /// <response code="404">If the client is not found</response>
    [HttpDelete("{userId}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid userId)
    {
        var command = new DeleteUserCommand(userId);
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Retrieves a specific client by their ID
    /// </summary>
    /// <param name="userId">The unique identifier of the client</param>
    /// <returns>The client details</returns>
    /// <response code="200">Returns the client details</response>
    /// <response code="404">If the client is not found</response>
    [HttpGet("{userId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne([FromRoute] Guid userId)
    {
        var client = await _mediator.Send(new GetUserByIdQuery(userId));
        return Ok(client);
    }

    /// <summary>
    /// Retrieves a paginated list of all clients
    /// </summary>
    /// <param name="query">Query parameters for filtering and pagination</param>
    /// <returns>A paginated list of clients</returns>
    /// <response code="200">Returns the list of clients</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
    {
        query.Role = RoleConstants.Client;
        var clients = await _mediator.Send(query);
        Response.AddPaginationHeader(clients.MetaData);
        return Ok(clients);
    }
}