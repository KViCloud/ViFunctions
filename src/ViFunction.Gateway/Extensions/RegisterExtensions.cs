using Refit;
using ViFunction.Gateway.Application.Commands.Handlers;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Extensions;

public static class RegisterExtensions
{
    public static void RegisterAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(BuildCommandHandler).Assembly); });

        var builderUrl =  builder.Configuration["Services:BuilderUrl"];
        var dataServiceUrl = builder.Configuration["Services:DataServiceUrl"];
        var deployerUrl = builder.Configuration["Services:DeployerUrl"];

        builder.Services.AddRefitClient<IImageBuilder>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(builderUrl!));
        
        builder.Services.AddRefitClient<IDataService>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(dataServiceUrl!));

        builder.Services.AddRefitClient<IDeployer>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(deployerUrl!));
    }
}