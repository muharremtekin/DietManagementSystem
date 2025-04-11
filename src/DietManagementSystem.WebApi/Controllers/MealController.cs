using DietManagementSystem.Application.Features.Meal.Commands.Create;
using DietManagementSystem.Application.Features.Meal.Commands.Delete;
using DietManagementSystem.Application.Features.Meal.Commands.Update;
using DietManagementSystem.Application.Features.Meal.Queries.GetAllMeals;
using DietManagementSystem.Application.Features.Meal.Queries.GetMealQuery;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing meals and their operations
    /// </summary>
    [Authorize(Policy = "DietitianPolicy")]
    [Route("api/v{version:apiVersion}/meals")]
    [ApiController]
    [ApiVersion("1.0")]
    public class MealController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the MealController
        /// </summary>
        /// <param name="mediator">The mediator for handling commands and queries</param>
        public MealController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Retrieves all meals based on the query parameters
        /// </summary>
        /// <param name="query">Query parameters for filtering meals</param>
        /// <returns>A list of meals</returns>
        /// <response code="200">Returns the list of meals</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMealsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        /// <summary>
        /// Retrieves a specific meal by its ID
        /// </summary>
        /// <param name="id">The unique identifier of the meal</param>
        /// <returns>The meal details</returns>
        /// <response code="200">Returns the meal details</response>
        /// <response code="404">If the meal is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOne([FromRoute] Guid id) => Ok(await _mediator.Send(new GetMealQuery(id)));

        /// <summary>
        /// Creates a new meal
        /// </summary>
        /// <param name="command">The meal details to create</param>
        /// <returns>A 201 Created response</returns>
        /// <response code="201">If the meal was created successfully</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateMealCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        /// <summary>
        /// Updates an existing meal
        /// </summary>
        /// <param name="id">The unique identifier of the meal to update</param>
        /// <param name="command">The updated meal details</param>
        /// <returns>A 204 No Content response</returns>
        /// <response code="204">If the meal was updated successfully</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If the meal is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMealCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Deletes a meal
        /// </summary>
        /// <param name="id">The unique identifier of the meal to delete</param>
        /// <returns>A 204 No Content response</returns>
        /// <response code="204">If the meal was deleted successfully</response>
        /// <response code="404">If the meal is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteMealCommand(id));
            return NoContent();
        }
    }
}
