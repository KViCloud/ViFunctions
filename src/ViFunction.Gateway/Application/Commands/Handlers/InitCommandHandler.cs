using MediatR;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Application.Commands.Handlers
{
    public class InitCommandHandler(
        IDataService dataService
    ) : IRequestHandler<InitCommand, Result>
    {
        public async Task<Result> Handle(InitCommand request, CancellationToken cancellationToken)
        {
            var functionDto = new FunctionDto()
            {
                Cluster = "Default",
                Image = request.FunctionName.ToLower(),
                Language = request.Language,
                LanguageVersion = request.Version,
                Name = request.FunctionName,
                UserId = "TestUser",
                Message = "New"
            };
            var response = await dataService.CreateFunctionAsync(functionDto);

            return response.IsSuccessStatusCode
                ? new Result(true, "Initialization Successful")
                : new Result(false, response.Error.Content);
        }
    }
}