using MediatR;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Application.Commands.Handlers;

public class DeployCommandHandler(
    IKubeOps kubeOps,
    IStore store,
    ILogger<DeployCommandHandler> logger)
    : IRequestHandler<DeployCommand, Result>
{
    public async Task<Result> Handle(DeployCommand command, CancellationToken cancellationToken)
    {
        var functionDto = await store.GetFunctionByIdAsync(command.FunctionId);

        logger.LogInformation("Handling deployment for function: {FunctionName}", functionDto.Name);
        var apiResponse = await kubeOps.DeployAsync(new DeployDto(
            functionDto.KubernetesName, 
            functionDto.Image,
            functionDto.CpuRequest,
            functionDto.MemoryRequest,
            functionDto.CpuLimit,
            functionDto.MemoryLimit
            ));

        var result = apiResponse.IsSuccessStatusCode ? new Result() : new Result(false, apiResponse.Error!.Content);
        logger.LogInformation("{FunctionName} build result: {result}", functionDto.Name,
            System.Text.Json.JsonSerializer.Serialize(result));
        return result;
    }
}