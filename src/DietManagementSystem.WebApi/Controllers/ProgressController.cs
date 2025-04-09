using DietManagementSystem.Application.Features.Progress.Commands.Create;
using DietManagementSystem.Application.Features.Progress.Commands.Delete;
using DietManagementSystem.Application.Features.Progress.Commands.Update;
using DietManagementSystem.Application.Features.Progress.Queries.GetAllProgressQuery;
using DietManagementSystem.Application.Features.Progress.Queries.GetProgressQuery;
using DietManagementSystem.Common.Constants;
using DietManagementSystem.WebApi.Controllers.Base;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DietManagementSystem.WebApi.Controllers;
[Route(RouteConstants.progress)]
[ApiController]
public class ProgressController : BaseController
{
    public ProgressController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProgress([FromRoute] Guid id)
    {
        var progress = await _mediator.Send(new GetProgressQuery(id));
        return Ok(progress);
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProgress([FromQuery] Guid dietPlanId)
    {
        var progress = await _mediator.Send(new GetAllProgressQuery(dietPlanId));
        return Ok(progress);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProgress([FromBody] CreateProgressCommand command)
    {
        await _mediator.Send(command);
        return Created();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProgress([FromRoute] Guid id, [FromBody] UpdateProgressCommand command)
    {
        command.Id = id;
        await _mediator.Send(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProgress([FromRoute] Guid id)
    {
        await _mediator.Send(new DeleteProgressCommand(id));
        return NoContent();
    }
}

