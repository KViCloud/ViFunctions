using MediatR;
using Microsoft.AspNetCore.Mvc;
using ViFunction.Gateway.Application.Commands;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Apis;

[Route("api/[controller]")]
[ApiController]
public class FunctionsController(IMediator mediator, IDataService dataService): ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await dataService.GetFunctionsAsync();
        return Ok(result);
    }
    
    [HttpPost("build")]
    public async Task<IActionResult> Build([FromForm] BuildCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
    
    [HttpPost("init")]
    public async Task<IActionResult> Build( [FromBody] InitCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
    
    [HttpPost("deploy")]
    public async Task<IActionResult> Deploy([FromBody] DeployCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}