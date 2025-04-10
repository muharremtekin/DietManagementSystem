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
    [Authorize(Policy = "DietitianPolicy")]
    [Route("api/meals")]
    [ApiController]
    public class MealController : BaseController
    {
        public MealController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllMealsQuery query)
        {
            return Ok(await _mediator.Send(query));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne([FromRoute] Guid id)
            => Ok(await _mediator.Send(new GetMealQuery(id)));


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMealCommand command)
        {
            await _mediator.Send(command);
            return Created();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateMealCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteMealCommand(id));
            return NoContent();
        }

    }
}
