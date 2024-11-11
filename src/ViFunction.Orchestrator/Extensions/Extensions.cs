using Refit;
using ViFunction.Orchestrator.Application.Commands.Handlers;
using ViFunction.Orchestrator.Application.Services.BuildServices;
using ViFunction.Orchestrator.Application.Services.DeployServices;

namespace ViFunction.Orchestrator.Extensions;

public static class Extensions
{
    public static void RegisterAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(BuildCommandHandler).Assembly); });

        var goBuilderUrl = Environment.GetEnvironmentVariable("Services__GoBuilderUrl");
        var pythonBuilderUrl = Environment.GetEnvironmentVariable("Services__PythonBuilderUrl");
        var deployerUrl = Environment.GetEnvironmentVariable("Services__DeployerUrl");

        builder.Services.AddRefitClient<IGoBuilder>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(goBuilderUrl!));

        builder.Services.AddRefitClient<IPythonBuilder>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(pythonBuilderUrl!));
        
        builder.Services.AddRefitClient<IDeployer>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(deployerUrl!));
    }
}