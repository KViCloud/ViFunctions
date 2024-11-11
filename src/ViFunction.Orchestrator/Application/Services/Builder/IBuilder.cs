using Refit;

namespace ViFunction.Orchestrator.Application.Services.Builder;

public interface IBuilder
{
    [Multipart]
    [Post("/build")]
    Task<IApiResponse> BuildAsync([AliasAs("image_name")] string functionName,
        [AliasAs("files")] IEnumerable<StreamPart> files);
}