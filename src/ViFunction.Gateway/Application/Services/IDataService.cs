using System.ComponentModel.DataAnnotations;
using Refit;

namespace ViFunction.Gateway.Application.Services;

public interface IDataService
{
    [Get("/api/functions/{id}")]
    Task<FunctionDto> GetFunctionAsync(int id);
    
    [Get("/api/functions")]
    Task<List<FunctionDto>> GetFunctionsAsync();

    [Post("/api/functions")]
    Task<ApiResponse<FunctionDto>> CreateFunctionAsync([Body] FunctionDto functionDto);

    [Put("/api/functions/{id}")]
    Task UpdateFunctionAsync(int id, [Body] FunctionDto functionDto);

    [Delete("/api/functions/{id}")]
    Task DeleteFunctionAsync(int id);
}

public class FunctionDto
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; }
    [MaxLength(200)] public string Image { get; set; }
    [MaxLength(50)] public string Language { get; set; }
    [MaxLength(10)] public string LanguageVersion { get; set; }
    [MaxLength(100)] public string Cluster { get; set; }
    [MaxLength(50)] public string UserId { get; set; }
    [MaxLength(50)] public Status Status { get; set; }
    [MaxLength(500)] public string Message { get; set; }
}

public enum Status
{
    None,
    Built,
    Deployed,
}