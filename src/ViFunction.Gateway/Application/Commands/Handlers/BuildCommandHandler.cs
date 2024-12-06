using MediatR;
using Refit;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Application.Commands.Handlers;

public class BuildCommandHandler(
    IImageBuilder imageBuilder,
    IStore store,
    ILogger<BuildCommandHandler> logger)
    : IRequestHandler<BuildCommand, Result>
{
    public async Task<Result> Handle(BuildCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("Handling build request for function: {FunctionId}", command.FunctionId);

        var funcDto = await store.GetFunctionByIdAsync(command.FunctionId);

        var streamParts = new List<StreamPart>();
        foreach (var file in command.Files)
        {
            if (file.Length <= 0) continue;
            logger.LogInformation("Processing file: {FileName}", file.FileName);
            streamParts.Add(new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType));
        }

        var apiResponse = await imageBuilder.BuildAsync(funcDto.KubernetesName, funcDto.LanguageVersion, streamParts);

        if (apiResponse.IsSuccessStatusCode)
        {
          var apiResponseContent = apiResponse.Content[1..^1];;
          await store.UpdateFunctionAsync(funcDto.Id, new(FunctionStatus.Built, apiResponseContent, "Built"));
        }

        var result = apiResponse.IsSuccessStatusCode ? new Result() : new Result(false, apiResponse.Error!.Content);
        return result;
    }
}