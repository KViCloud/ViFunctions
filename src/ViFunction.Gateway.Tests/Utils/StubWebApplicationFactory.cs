using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using ViFunction.Gateway.Application.Services;

namespace ViFunction.Gateway.Tests.Utils;

public class StubWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            RemoveService(services, typeof(IImageBuilder));
            RemoveService(services, typeof(IDeployer));
            RemoveService(services, typeof(IDataService));

            services.AddSingleton<IImageBuilder,StubImageBuilder>();
            services.AddSingleton<IDeployer,StubDeployer>();
            services.AddSingleton<IDataService,StubDataService>();
        });

        base.ConfigureWebHost(builder);
    }

    private static void RemoveService(IServiceCollection services, Type serviceType)
    {
        var descriptor = services.FirstOrDefault(d =>
            d.ServiceType == serviceType);
        if (descriptor != null) services.Remove(descriptor);
    }
}