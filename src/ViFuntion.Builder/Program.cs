using ViFuntion.Builder.Handler;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLogging();
builder.Services.AddSingleton<IApiRequestHandler, ApiRequestHandler>();

var app = builder.Build();

app.MapPost("/build", async (IApiRequestHandler handler, HttpRequest request) =>
{
    var buildResult = await handler.HandleApiRequest(request);

    return buildResult.Success ? Results.Ok(buildResult.Message) : Results.BadRequest(buildResult.Message);

});

app.Run();