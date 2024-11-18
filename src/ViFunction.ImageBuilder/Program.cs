using ViFunction.ImageBuilder.Handler;

var builder = WebApplication.CreateBuilder(args);
builder.AddServiceDefaults();

builder.Services.AddLogging();
builder.Services.AddSingleton<IApiRequestHandler, ApiRequestHandler>();

var app = builder.Build();

app.MapPost("/build", async (IApiRequestHandler handler, HttpRequest request) =>
{
    var buildResult = await handler.HandleApiRequest(request);

    return buildResult.Success ? Results.Ok(buildResult.Message) : Results.BadRequest(buildResult.Message);

});

app.MapGet("/", () => "Healthy!");
app.Run();