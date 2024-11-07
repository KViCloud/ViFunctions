using FunctionsManager.Application.Commands.Handlers;
using FunctionsManager.Application.Services.BuildServices;
using Refit;

namespace FunctionsManager.Extensions;

public static class Extensions
{
    public static void RegisterAppServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(BuildRequestHandler).Assembly); });

        builder.Services.AddRefitClient<IGoBuilder>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://goBuilder"));

        builder.Services.AddRefitClient<IPythonBuilder>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("http://pythonbuilder"));
    }
}