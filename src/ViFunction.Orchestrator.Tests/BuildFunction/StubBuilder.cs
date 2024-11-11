using System.Net;
using Refit;
using ViFunction.Orchestrator.Application.Services.Builder;

namespace ViFunction.Orchestrator.Tests.BuildFunction;

public class StubBuilder : IBuilder
{
    public Task<IApiResponse> BuildAsync(string functionName, IEnumerable<StreamPart> files)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Build Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }
}