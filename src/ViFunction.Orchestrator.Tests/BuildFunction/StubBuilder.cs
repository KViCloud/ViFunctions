using System.Net;
using ViFunction.Orchestrator.Application.Services.BuildServices;
using Refit;

namespace ViFunction.Orchestrator.Tests.BuildFunction;

public class StubBuilder : IGoBuilder, IPythonBuilder
{
    public Task<IApiResponse> BuildAsync(string functionName, IEnumerable<StreamPart> files)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Build Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse>(response);
    }
}