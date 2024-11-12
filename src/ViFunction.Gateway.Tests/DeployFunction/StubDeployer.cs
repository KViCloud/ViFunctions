using System.Net;
using Refit;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Tests.DeployFunction;

public class StubDeployer : IDeployer
{
    public Task<IApiResponse> DeployAsync(DeployDto deployDto)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Deploy Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }
}