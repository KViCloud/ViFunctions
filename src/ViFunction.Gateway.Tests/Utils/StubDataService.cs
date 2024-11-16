using System.Net;
using Refit;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Tests.Utils;

public class StubDataService : IDataService
{
    public Task<FunctionDto> GetFunctionAsync(int id)
    {
        // Return a dummy FunctionDto object
        return Task.FromResult(new FunctionDto
        {
            Id = id,
            Name = "Dummy Function"
        });
    }

    public Task<List<FunctionDto>> GetFunctionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<FunctionDto>> CreateFunctionAsync(FunctionDto functionDto)
    {
        // Return a dummy ApiResponse with a FunctionDto object
        var response = new ApiResponse<FunctionDto>(
            new HttpResponseMessage(HttpStatusCode.OK),
            functionDto,
            new RefitSettings());

        return Task.FromResult(response);
    }

    public Task UpdateFunctionAsync(int id, FunctionDto functionDto)
    {
        // No operation for update
        return Task.CompletedTask;
    }

    public Task DeleteFunctionAsync(int id)
    {
        // No operation for delete
        return Task.CompletedTask;
    }
}