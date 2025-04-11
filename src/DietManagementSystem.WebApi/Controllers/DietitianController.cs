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

namespace DietManagementSystem.WebApi.Controllers
{
    /// <summary>
    /// Controller for managing dietitian accounts and operations
    /// </summary>
    [Authorize(Policy = "DietitianPolicy")]
    [Route("api/v{version:apiVersion}/dietitians")]
    [ApiController]
    [ApiVersion("1.0")]
    public class DietitianController : BaseController
    {
        /// <summary>
        /// Initializes a new instance of the DietitianController
        /// </summary>
        /// <param name="mediator">The mediator for handling commands and queries</param>
        public DietitianController(IMediator mediator) : base(mediator)
        {
        }

        /// <summary>
        /// Retrieves a specific dietitian by their ID
        /// </summary>
        /// <param name="userId">The unique identifier of the dietitian</param>
        /// <returns>The dietitian details</returns>
        /// <response code="200">Returns the dietitian details</response>
        /// <response code="404">If the dietitian is not found</response>
        [HttpGet("{userId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOne([FromRoute] Guid userId)
        {
            var result = await _mediator.Send(new GetUserByIdQuery(userId));
            return Ok(result);
        }

        /// <summary>
        /// Retrieves a paginated list of all dietitians
        /// </summary>
        /// <param name="query">Query parameters for filtering and pagination</param>
        /// <returns>A paginated list of dietitians</returns>
        /// <response code="200">Returns the list of dietitians</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll([FromQuery] GetAllUsersQuery query)
        {
            query.Role = RoleConstants.Dietitian;
            var result = await _mediator.Send(query);
            Response.AddPaginationHeader(result.MetaData);
            return Ok(result);
        }

        /// <summary>
        /// Creates a new dietitian account
        /// </summary>
        /// <param name="command">The dietitian details to create</param>
        /// <returns>A 201 Created response</returns>
        /// <response code="201">If the dietitian was created successfully</response>
        /// <response code="400">If the request is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            command.Role = RoleConstants.Dietitian;
            await _mediator.Send(command);
            return Created();
        }

        /// <summary>
        /// Updates an existing dietitian's details
        /// </summary>
        /// <param name="userId">The unique identifier of the dietitian to update</param>
        /// <param name="command">The updated dietitian details</param>
        /// <returns>A 204 No Content response</returns>
        /// <response code="204">If the dietitian was updated successfully</response>
        /// <response code="400">If the request is invalid</response>
        /// <response code="404">If the dietitian is not found</response>
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
        /// Deletes a dietitian account
        /// </summary>
        /// <param name="userId">The unique identifier of the dietitian to delete</param>
        /// <returns>A 204 No Content response</returns>
        /// <response code="204">If the dietitian was deleted successfully</response>
        /// <response code="404">If the dietitian is not found</response>
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
}
