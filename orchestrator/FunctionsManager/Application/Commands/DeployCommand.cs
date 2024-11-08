using MediatR;

namespace FunctionsManager.Application.Commands;

public class DeployCommand : IRequest<Result>
{
    public string FunctionName { get; set; }
    // Tier 1,2,3,4  Spec: cpu, ram,
}