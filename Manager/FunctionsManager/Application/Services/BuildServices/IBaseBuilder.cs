using Refit;

namespace FunctionsManager.Application.Services.BuildServices;

public interface IBaseBuilder
{
    [Multipart]
    [Get("/build")]
    Task<IApiResponse> BuildAsync(string functionName, [AliasAs("files")] IEnumerable<StreamPart> files);
}