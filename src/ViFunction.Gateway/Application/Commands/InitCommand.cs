using MediatR;

namespace ViFunction.Gateway.Application.Commands;

public class InitCommand : IRequest<Result>
{
    public string Language { get; set; }
    public string Version { get; set; }
    public string FunctionName { get; set; }
}