using MediatR;
using Refit;
using ViFunction.Orchestrator.Application.Services.Builder;

namespace ViFunction.Orchestrator.Application.Commands.Handlers;

public class BuildCommandHandler(
    IBuilder builder,
    ILogger<BuildCommandHandler> logger)
    : IRequestHandler<BuildCommand, Result>
{
    public async Task<Result> Handle(BuildCommand command, CancellationToken cancellationToken)
    {
        command.FunctionName = command.FunctionName.ToLower();
        logger.LogInformation("Handling build request for function: {FunctionName}", command.FunctionName);

        var streamParts = new List<StreamPart>();
        foreach (var file in command.Files)
        {
            if (file.Length <= 0) continue;
            logger.LogInformation("Processing file: {FileName}", file.FileName);
            streamParts.Add(new StreamPart(file.OpenReadStream(), file.FileName, file.ContentType));
        }

        var language = DetermineFunctionLanguage(streamParts);
        if (string.IsNullOrEmpty(language))
        {
            logger.LogWarning("Could not detect programming language for function: {FunctionName}",
                command.FunctionName);
            return new Result(false, "Cannot detect programming language.");
        }

        logger.LogInformation("Detected language: {Language}", language);
        var fileStream =
            File.OpenRead($"Application/Services/Builder/ContainerTemplates/{language}/Containerfile");
        streamParts.Add(new StreamPart(fileStream, "Containerfile", "text/plain"));

        var result = await BuildFunctionAsync(command, language, streamParts);

        logger.LogInformation("{FunctionName} build result: {result}", command.FunctionName,
            System.Text.Json.JsonSerializer.Serialize(result));

        if (result.IsSuccess)
        {
            // Add to database
            // functionId, functionName, Language, Version, UserId
        }

        return result;
    }

    private async Task<Result> BuildFunctionAsync(BuildCommand command, string language, List<StreamPart> streamParts)
    {
        var apiResponse = await builder.BuildAsync(command.FunctionName, streamParts);

        return apiResponse.IsSuccessStatusCode ? new Result() : new Result(false, apiResponse.Error!.Content);
    }

    private string DetermineFunctionLanguage(List<StreamPart> streamParts)
    {
        if (streamParts.Any(x => x.FileName.EndsWith(".py")))
            return "Python";

        if (streamParts.Any(x => x.FileName.EndsWith(".go")))
            return "Go";

        return null;
    }
}