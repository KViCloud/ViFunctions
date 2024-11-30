using ViFunction.KubeOps;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<KubernetesService>(); // Register the service

var app = builder.Build();

app.MapPost("/deploy", async (DeploymentRequest request, KubernetesService kubernetesService) =>
{
    await kubernetesService.DeployAsync(request);
    return Results.Ok("Deployment, HPA, and Service created successfully.");
});

app.MapPost("/destroy", async (string name, KubernetesService kubernetesService) =>
{
    await kubernetesService.RollbackAsync(name);
    return Results.Ok("Rollback of deployment, HPA, and Service completed successfully.");
});

app.Run();

namespace ViFunction.KubeOps
{
    public record DeploymentRequest(
        string Name,
        string Image,
        int Replicas,
        string CpuRequest,
        string MemoryRequest,
        string CpuLimit,
        string MemoryLimit);
}