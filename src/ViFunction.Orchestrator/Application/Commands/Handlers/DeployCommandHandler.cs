using System.Text.RegularExpressions;
using MediatR;
using ViFunction.Orchestrator.Application.Services.DeployServices;

namespace ViFunction.Orchestrator.Application.Commands.Handlers;

public class DeployCommandHandler(
    IDeployer deployer,
    ILogger<DeployCommandHandler> logger)
    : IRequestHandler<DeployCommand, Result>
{
    public async Task<Result> Handle(DeployCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling deployment for function: {FunctionName}", command.FunctionName);
        var cleanedFunctionName = Regex.Replace(command.FunctionName, "[^a-zA-Z0-9]", "").ToLower();
        var apiResponse = await deployer.DeployAsync(new DeployParams(
            cleanedFunctionName, 
            command.FunctionName));

        var result = apiResponse.IsSuccessStatusCode ? new Result() : new Result(false, apiResponse.Error!.Content);
        logger.LogInformation("{FunctionName} build result: {result}", command.FunctionName,
            System.Text.Json.JsonSerializer.Serialize(result));
        return result;
    }
}