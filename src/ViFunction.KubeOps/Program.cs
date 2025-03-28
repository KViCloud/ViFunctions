using ViFunction.KubeOps;
using ViFunction.KubeOps.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Services.AddSingleton<IOperation, Operation>();

builder.Services.AddHostedService<EventWatcher>();

var app = builder.Build();

app.MapPost("/deploy", async (DeploymentRequest request, IOperation ops) =>
{
    var success = await ops.DeployAsync(request);
    return Results.Ok(success);
});

app.MapDelete("/destroy/{name}", async (string name, IOperation ops) =>
{
    await ops.DestroyAsync(name);
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