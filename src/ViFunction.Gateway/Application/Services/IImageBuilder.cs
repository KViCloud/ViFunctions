using Refit;

namespace ViFunction.Gateway.Application.Services;

public interface IImageBuilder
{
    [Multipart]
    [Post("/build")]
    Task<IApiResponse<string>> BuildAsync(
        [AliasAs("image")] string imageName,
        [AliasAs("version")] string version,
        [AliasAs("files")] IEnumerable<StreamPart> files);
}