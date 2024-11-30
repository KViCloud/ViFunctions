using MediatR;

namespace ViFunction.Gateway.Application.Commands;

public class DeployCommand : IRequest<Result>
{
    public Guid FunctionId { get; set; }
}