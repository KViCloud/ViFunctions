using MediatR;
using ViFunction.Store.Application.Dtos;

namespace ViFunction.Store.Application.Requests;

public record CreateFunctionCommand : IRequest<FunctionDto>
{
    public string Name { get; init; }
    public string Language { get; set; }
    public string LanguageVersion { get; set; }
    public string UserId { get; set; }
    public string Cluster { get; set; }
    public string Tier { get; set; }
}