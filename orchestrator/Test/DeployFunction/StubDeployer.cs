using System.Net;
using FunctionsManager.Application.Services.DeployServices;
using Refit;

namespace Test.DeployFunction;

public class StubDeployer : IDeployer
{
    public Task<IApiResponse> DeployAsyncAsync(string name, string image)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Deploy Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }
}