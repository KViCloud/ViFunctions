using ViFunction.EdgeRouter.Endpoints;
using ViFunction.EdgeRouter.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterAppServices();

var app = builder.Build();

app.MapGet("/", () => "ViFunction Edge Router is running...");
app.MapProxy();

app.Run();