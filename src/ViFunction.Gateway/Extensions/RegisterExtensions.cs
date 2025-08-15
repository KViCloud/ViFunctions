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
        var storeUrl = builder.Configuration["Services:StoreUrl"];
        var kubeOpsUrl = builder.Configuration["Services:KubeOpsUrl"];

        builder.Services.AddRefitClient<IImageBuilder>()
            .ConfigureHttpClient(c =>
            {
                c.BaseAddress = new Uri(builderUrl!);
                c.Timeout = TimeSpan.FromMinutes(10);
            });
        
        builder.Services.AddRefitClient<IStore>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(storeUrl!));

        builder.Services.AddRefitClient<IKubeOps>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(kubeOpsUrl!));

//        var allowedOrigins = new[]
//        {
//            "https://vifunction-ui.openvicloud.com",
//            "http://localhost:3000",
//            "http://localhost:8080"
//        };

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
    }
}