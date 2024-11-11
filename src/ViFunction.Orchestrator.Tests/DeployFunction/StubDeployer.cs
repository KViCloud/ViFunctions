using System.Net;
using ViFunction.Orchestrator.Application.Services.DeployServices;
using Refit;

namespace ViFunction.Orchestrator.Tests.DeployFunction;

public class StubDeployer : IDeployer
{
    public Task<IApiResponse> DeployAsync(DeployParams deployParams)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Deploy Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }
}