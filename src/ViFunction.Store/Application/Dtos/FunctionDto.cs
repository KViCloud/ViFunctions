using ViFunction.Store.Application.Entities;

namespace ViFunction.Store.Application.Dtos;

public record FunctionDto(
    Guid Id,
    string Name,
    string Image,
    string Language,
    string LanguageVersion,
    string Cluster,
    string UserId,
    string FunctionStatus,
    string Message,
    string KubernetesName,
    string CpuRequest,
    string MemoryRequest,
    string CpuLimit,
    string MemoryLimit
);