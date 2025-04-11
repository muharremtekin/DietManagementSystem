using DietManagementSystem.Application.Features.DietPlan.Commands.Create;
using DietManagementSystem.Application.Features.DietPlan.Commands.Delete;
using DietManagementSystem.Application.Features.DietPlan.Commands.Update;
using DietManagementSystem.Application.Features.DietPlan.Queries.GetAllDietPlansQuery;
using DietManagementSystem.Application.Features.DietPlan.Queries.GetDietPlanQuery;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;

/// <summary>
/// Controller for managing diet plans and their operations
/// </summary>
[Authorize(Policy = "ManageDietPlans")]
[Route("api/v{version:apiVersion}/diet-plans")]
[ApiController]
[ApiVersion("1.0")]
public class DietPlanController : BaseController
{
    /// <summary>
    /// Initializes a new instance of the DietPlanController
    /// </summary>
    /// <param name="mediator">The mediator for handling commands and queries</param>
    public DietPlanController(IMediator mediator) : base(mediator)
    {
    }

    /// <summary>
    /// Creates a new diet plan
    /// </summary>
    /// <param name="command">The diet plan details to create</param>
    /// <returns>A 201 Created response</returns>
    /// <response code="201">If the diet plan was created successfully</response>
    /// <response code="400">If the request is invalid</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateDietPlanCommmand command)
    {
        command.RequesterInfo = (UserId, UserRole);
        await _mediator.Send(command);
        return Created();
    }

    /// <summary>
    /// Retrieves a specific diet plan by dietitian ID
    /// </summary>
    /// <param name="dietitianId">The unique identifier of the dietitian</param>
    /// <returns>The diet plan details</returns>
    /// <response code="200">Returns the diet plan details</response>
    /// <response code="404">If the diet plan is not found</response>
    [HttpGet("{dietitianId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOne([FromRoute] Guid dietitianId)
    {
        var result = await _mediator.Send(new GetDietPlanQuery(dietitianId));
        return Ok(result);
    }

    /// <summary>
    /// Retrieves all diet plans based on the requester's role and ID
    /// </summary>
    /// <param name="query">Query parameters for filtering diet plans</param>
    /// <returns>A list of diet plans</returns>
    /// <response code="200">Returns the list of diet plans</response>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDietPlansQuery query)
    {
        query.RequesterInfo = (UserId, UserRole);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// Updates an existing diet plan
    /// </summary>
    /// <param name="id">The unique identifier of the diet plan to update</param>
    /// <param name="command">The updated diet plan details</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the diet plan was updated successfully</response>
    /// <response code="400">If the request is invalid</response>
    /// <response code="404">If the diet plan is not found</response>
    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDietPlanCommand command)
    {
        command.RequesterInfo = (UserId, UserRole);
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    /// <summary>
    /// Deletes a diet plan
    /// </summary>
    /// <param name="id">The unique identifier of the diet plan to delete</param>
    /// <returns>A 204 No Content response</returns>
    /// <response code="204">If the diet plan was deleted successfully</response>
    /// <response code="404">If the diet plan is not found</response>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteDietPlanCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}