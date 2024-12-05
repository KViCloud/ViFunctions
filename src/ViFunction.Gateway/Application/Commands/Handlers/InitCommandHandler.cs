using MediatR;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Application.Commands.Handlers
{
    public class InitCommandHandler(
        IStore store
    ) : IRequestHandler<InitCommand, Result>
    {
        public async Task<Result> Handle(InitCommand request, CancellationToken cancellationToken)
        {
            var response = await store.CreateFunctionAsync(new CreateFunctionRequest(
                Name: request.FunctionName,
                Language: request.Language,
                LanguageVersion: request.Version,
                UserId: "TestUser",
                Cluster: "Default",
                Tier: "Standard"
            ));

            return response.IsSuccessStatusCode
                ? new Result(true, "Initialization Successful")
                : new Result(false, response.Error.Content);
        }
    }
}