using Refit;

namespace ViFunction.Orchestrator.Application.Services.Storage;

public interface IStorage
{
    [Get("/api/functions/{id}")]
    Task<FunctionDto> GetFunctionAsync(int id);

    [Post("/api/functions")]
    Task<FunctionDto> CreateFunctionAsync([Body] FunctionDto functionDto);

    [Put("/api/functions/{id}")]
    Task UpdateFunctionAsync(int id, [Body] FunctionDto functionDto);

    [Delete("/api/functions/{id}")]
    Task DeleteFunctionAsync(int id);
}