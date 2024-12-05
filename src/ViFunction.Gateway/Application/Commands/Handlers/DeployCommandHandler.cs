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

        if (!apiResponse.IsSuccessStatusCode)
        {
            return new Result(false, apiResponse.Error!.Content);
        }

        if (apiResponse.Content)
        {
            logger.LogInformation("{FunctionName} build success", functionDto.Name);
            await store.UpdateFunctionAsync(functionDto.Id, new(FunctionStatus.Deployed));
        }
        else
        {
            logger.LogInformation("{FunctionName} build failed, auto rollback", functionDto.Name);
            await store.UpdateFunctionAsync(functionDto.Id, new(
                Status: FunctionStatus.Deployed,
                Message: "Build fail, auto rollback"));
        }

        return new Result();
    }
}