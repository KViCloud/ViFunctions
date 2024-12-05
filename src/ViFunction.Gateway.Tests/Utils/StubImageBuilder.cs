using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Refit;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Tests.Utils;

public class StubImageBuilder : IImageBuilder
{
    public Task<IApiResponse<string>> BuildAsync(string imageName, string version, IEnumerable<StreamPart> files)
    {
        var response = new ApiResponse<string>(new HttpResponseMessage(HttpStatusCode.OK), "Build Successful",
            new RefitSettings());
        return Task.FromResult<IApiResponse<string>>(response);
    }
}