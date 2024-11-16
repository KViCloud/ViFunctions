using Refit;

namespace ViFunction.Gateway.Application.Services;

public interface IImageBuilder
{
    [Multipart]
    [Post("/build")]
    Task<IApiResponse> BuildAsync(
        [AliasAs("imageName")] string imageName,
        [AliasAs("version")] string version,
        [AliasAs("files")] IEnumerable<StreamPart> files);
}