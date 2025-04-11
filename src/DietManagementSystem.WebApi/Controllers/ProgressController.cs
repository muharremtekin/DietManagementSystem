using DietManagementSystem.Application.Features.Progress.Commands.Create;
using DietManagementSystem.Application.Features.Progress.Commands.Delete;
using DietManagementSystem.Application.Features.Progress.Commands.Update;
using DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;
using DietManagementSystem.Application.Features.Progress.Queries.GetProgressQuery;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;

/// <summary>
/// Controller for managing client progress tracking and operations
/// </summary>
[Authorize(Policy = "DietitianPolicy")]
[Route("api/v{version:apiVersion}/progresses")]
[ApiController]
[ApiVersion("1.0")]
public class ProgressController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the ProgressController
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries</param>
    public ProgressController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Retrieves a specific progress entry by its ID
    /// </summary>
    /// <param name="id">The unique identifier of the progress entry</param>
    /// <returns>The progress details</returns>
    /// <response code="200">Returns the progress details</response>
    /// <response code="404">If the progress entry is not found</response>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProgress([FromRoute] Guid id)
    {
        var progress = await _mediator.Send(new GetProgressQuery(id));
        return Ok(progress);
    }

    /// <summary>
    /// Retrieves all progress entries based on the query parameters
    /// </summary>
    /// <param name="query">Query parameters for filtering progress entries</param>
    /// <returns>A list of progress entries</returns>
    /// <response code="200">Returns the list of progress entries</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllProgress([FromQuery] GetAllProgressQuery query)
    {
        var progress = await _mediator.Send(query);
        return Ok(progress);
    }

    /// <summary>
    /// Creates a new progress entry
    /// </summary>
    /// <param name="command">The progress details to create</param>
    /// <returns>A 201 Created response</returns>
    /// <response code="201">If the progress entry was created successfully</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateProgress([FromBody] CreateProgressCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    /// <summary>
    /// Updates an existing progress entry
    /// </summary>
    /// <param name="id">The unique identifier of the progress entry to update</param>
    /// <param name="command">The updated progress details</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the progress entry was updated successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the progress entry is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateProgress([FromRoute] Guid id, [FromBody] UpdateProgressCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a progress entry
    /// </summary>
    /// <param name="id">The unique identifier of the progress entry to delete</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the progress entry was deleted successfully</response>
    /// <response code="404">If the progress entry is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProgress([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteProgressCommand(id));
        return NoContent();
    }
}

