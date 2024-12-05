using System.Runtime.CompilerServices;
using ViFunction.KubeOps;
[assembly: InternalsVisibleTo("ViFunction.KubeOps.Tests")]
var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<KubernetesService>(); // Register the service

var app = builder.Build();

app.MapPost("/deploy", async (DeploymentRequest request, KubernetesService kubernetesService) =>
{
    var success = await kubernetesService.DeployAsync(request);
    return Results.Ok(success);
});

app.MapDelete("/destroy/{name}", async (string name, KubernetesService kubernetesService) =>
{
    await kubernetesService.DestroyAsync(name);
    return Results.Ok("Destroy successfully.");
});

app.Run();

namespace ViFunction.KubeOps
{
    public record DeploymentRequest(
        string Name,
        string Namespace,
        string Image,
        int Replicas,
        string CpuRequest,
        string MemoryRequest,
        string CpuLimit,
        string MemoryLimit);
}