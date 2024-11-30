using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Tests.Utils;

public class StubKubeOps : IKubeOps
{
    public Task<IApiResponse> RollbackAsync(string name)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Deploy Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }

    Task<IApiResponse> IKubeOps.DeployAsync(DeployDto request)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Deploy Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }
}