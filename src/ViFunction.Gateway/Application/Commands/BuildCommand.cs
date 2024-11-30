using MediatR;

namespace ViFunction.Gateway.Application.Commands;

public class BuildCommand : IRequest<Result>
{
    public Guid FunctionId { get; set; }
    public ICollection<IFormFile> Files { get; set; }
}