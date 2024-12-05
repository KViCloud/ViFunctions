using MediatR;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Application.Commands.Handlers;

public class DestroyCommandHandler(
    IKubeOps kubeOps,
    IStore store,
    ILogger<DestroyCommandHandler> logger)
    : IRequestHandler<DestroyCommand, Result>
{
    public async Task<Result> Handle(DestroyCommand command, CancellationToken cancellationToken)
    {
        var functionDto = await store.GetFunctionByIdAsync(command.FunctionId);

        logger.LogInformation("Destroy function: {FunctionName}", functionDto.Name);
        var apiResponse = await kubeOps.DestroyAsync(functionDto.KubernetesName);

        if (!apiResponse.IsSuccessStatusCode)
        {
            logger.LogInformation("Destroy {FunctionName} got error", apiResponse.Error!.Content);
            return new Result(false, apiResponse.Error!.Content);
        }

        await store.UpdateFunctionAsync(functionDto.Id, new(
            Status: FunctionStatus.Destroyed,
            Message: "Destroyed, waiting billing then auto delete"));
        logger.LogInformation("{FunctionName} Destroy success", functionDto.Name);

        return new Result();
    }
}