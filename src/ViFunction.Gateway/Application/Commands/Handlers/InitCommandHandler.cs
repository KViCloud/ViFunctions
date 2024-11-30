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
            var response = await store.CreateFunctionAsync(new CreateFunctionRequest()
            {
                Cluster = "Default",
                Language = request.Language,
                LanguageVersion = request.Version,
                Name = request.FunctionName,
                UserId = "TestUser",
            });

            return response.IsSuccessStatusCode
                ? new Result(true, "Initialization Successful")
                : new Result(false, response.Error.Content);
        }
    }
}