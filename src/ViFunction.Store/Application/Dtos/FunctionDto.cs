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
    FunctionStatus Status,
    string Message,
    string KubernetesName,
    string CpuRequest,
    string MemoryRequest,
    string CpuLimit,
    string MemoryLimit
);