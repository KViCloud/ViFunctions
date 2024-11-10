using MediatR;

namespace FunctionsOrchestrator.Application.Commands;

public class BuildCommand : IRequest<Result>
{
    public string Version { get; set; }
    public ICollection<IFormFile> Files { get; set; }
    public string FunctionName { get; set; }
}