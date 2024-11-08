using MediatR;

namespace FunctionsManager.Application.Commands.Handlers;

public class DeployCommandHandler(
    ILogger<DeployCommandHandler> logger)
    : IRequestHandler<DeployCommand, Result>
{
    public async Task<Result> Handle(DeployCommand command, CancellationToken cancellationToken)
    {
        return new Result();
    }
}