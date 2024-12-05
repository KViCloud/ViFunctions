using MediatR;

namespace ViFunction.Gateway.Application.Commands;

public class DestroyCommand : IRequest<Result>
{
    public Guid FunctionId { get; set; }
}