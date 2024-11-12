using Refit;
using ViFunction.Gateway.Application.Commands.Handlers;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Extensions;

public static class Extensions
{
    public static void RegisterAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(BuildCommandHandler).Assembly); });

        var builderUrl = Environment.GetEnvironmentVariable("Services__BuilderUrl");
        var storageUrl = Environment.GetEnvironmentVariable("Services__StorageUrl");
        var deployerUrl = Environment.GetEnvironmentVariable("Services__DeployerUrl");

        builder.Services.AddRefitClient<IBuilder>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(builderUrl!));
        builder.Services.AddRefitClient<IStorage>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(storageUrl!));

        builder.Services.AddRefitClient<IDeployer>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(deployerUrl!));
    }
}