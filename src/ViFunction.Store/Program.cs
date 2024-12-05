using System.Text.Json.Serialization;
using MediatR;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Repositories;
using ViFunction.Store.Application.Requests;
using ViFunction.Store.Infras;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssembly(typeof(ViFunction.Store.Program).Assembly); });

builder.Services.AddScoped<IRepository<Function>, FunctionRepository>();

builder.Services.AddDbContext<FunctionsContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("FunctionsDatabase"),
        new MySqlServerVersion(new Version(8, 0)))); // Adjust version as needed

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

ApplyMigrations(app);

app.MapGet("/", () => "Healthy!");

app.MapGet("/api/functions", async (IMediator mediator) =>
{
    var result = await mediator.Send(new GetAllFunctionsQuery());
    return Results.Ok(result);
});

app.MapGet("/api/functions/{id}", async (Guid id, IMediator mediator) =>
{
    var result = await mediator.Send(new GetFunctionByIdQuery(id));
    return result != null ? Results.Ok(result) : Results.NotFound();
});

app.MapPost("/api/functions", async (CreateFunctionCommand command, IMediator mediator) =>
{
    var result = await mediator.Send(command);
    return Results.Created($"/api/functions/{result.Id}", result);
});

app.MapPut("/api/functions/{id}", async (Guid id, UpdateFunctionCommand command, IMediator mediator) =>
{
    command.Id = id;
    await mediator.Send(command);
    return Results.Ok();
});

app.MapDelete("/api/functions/{id}", async (Guid id, IMediator mediator) =>
{
    await mediator.Send(new DeleteFunctionCommand(id));
    return Results.Ok();
});

app.Run();
return;

void ApplyMigrations(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<ViFunction.Store.Program>>();
    var context = scope.ServiceProvider.GetRequiredService<FunctionsContext>();

    if (context.Database.ProviderName!.Contains("InMemory"))
        return;
    
    try
    {
        logger.LogInformation("Starting database migration.");
        context.Database.Migrate();
        logger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw;
    }
}

namespace ViFunction.Store
{
    public partial class Program
    {
    }
}