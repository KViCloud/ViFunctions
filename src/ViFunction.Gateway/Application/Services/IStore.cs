using Refit;

namespace ViFunction.Gateway.Application.Services;

public interface IStore
{
    [Get("/api/functions")]
    Task<List<FunctionDto>> GetAllFunctionsAsync();

    [Get("/api/functions/{id}")]
    Task<FunctionDto> GetFunctionByIdAsync(Guid id);

    [Post("/api/functions")]
    Task<ApiResponse<FunctionDto>> CreateFunctionAsync([Body] CreateFunctionRequest request);

    [Put("/api/functions/{id}")]
    Task<IApiResponse> UpdateFunctionAsync(Guid id, [Body] UpdateFunctionRequest request);

    [Delete("/api/functions/{id}")]
    Task<IApiResponse> DeleteFunctionAsync(Guid id);
}

public record CreateFunctionRequest(
    string Name,
    string Language,
    string LanguageVersion,
    string UserId,
    string Cluster,
    string Tier);


public record UpdateFunctionRequest(
    FunctionStatus Status,
    string Image = "",
    string Message = "");

public record FunctionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public string Language { get; set; }
    public string LanguageVersion { get; set; }
    public string Cluster { get; set; }
    public string UserId { get; set; }
    public FunctionStatus Status { get; set; }
    public string Message { get; set; }
    public string KubernetesName { get; set; }
    public string CpuRequest { get; set; }
    public string MemoryRequest { get; set; }
    public string CpuLimit { get; set; }
    public string MemoryLimit { get; set; }
    public string Tier { get; set; }
}

public enum FunctionStatus
{
    None,
    Built,
    Deployed,
    Destroyed,
}