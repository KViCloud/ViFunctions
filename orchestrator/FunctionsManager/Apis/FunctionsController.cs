using FunctionsManager.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FunctionsManager.Apis;

[Route("api/[controller]")]
[ApiController]
public class FunctionsController(IMediator mediator): ControllerBase
{
    [HttpPost("build")]
    public async Task<IActionResult> Build([FromForm] BuildCommand command)
    {
        var result = await mediator.Send(command);
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
    [HttpPost("deploy/{functionName}")]
    public async Task<IActionResult> Deploy(string functionName)
    {
        var result = await mediator.Send(new DeployCommand()
        {
            FunctionName = functionName
        });
        return result.IsSuccess ? Ok() : BadRequest(result);
    }
}