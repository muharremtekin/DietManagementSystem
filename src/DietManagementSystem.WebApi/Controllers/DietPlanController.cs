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

[Authorize(Policy = "ManageDietPlans")]
[Route("api/diet-plans")]
[ApiController]
public class DietPlanController : BaseController
{
    public DietPlanController(IMediator mediator) : base(mediator)
    {
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDietPlanCommmand command)
    {
        command.RequesterInfo = (UserId, UserRole);
        await _mediator.Send(command);
        return Created();
    }

    [HttpGet("{dietitianId}")]
    public async Task<IActionResult> GetOne([FromRoute] Guid dietitianId)
    {
        var result = await _mediator.Send(new GetDietPlanQuery(dietitianId));
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] GetAllDietPlansQuery query)
    {
        // giriş yapan kullanıcıya göre çeşitli business kuralları işletilebilir
        // fakat case study için bu kadarı yeterli
        query.RequesterInfo = (UserId, UserRole);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateDietPlanCommand command)
    {
        command.RequesterInfo = (UserId, UserRole);
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var command = new DeleteDietPlanCommand(id);
        await _mediator.Send(command);
        return NoContent();
    }
}