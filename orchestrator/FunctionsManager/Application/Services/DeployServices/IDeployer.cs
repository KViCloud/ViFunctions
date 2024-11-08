using Refit;

namespace FunctionsManager.Application.Services.DeployServices;

public interface IDeployer
{
    [Post("/deploy")]
    Task<IApiResponse> DeployAsyncAsync(
        [AliasAs("name")] string name,
        [AliasAs("image")] string image);
}