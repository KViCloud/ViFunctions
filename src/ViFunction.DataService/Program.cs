using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using ViFunction.DataService.Data;
using ViFunction.DataService.Models;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.AddDbContext<FunctionsContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("FunctionsDatabase"),
        new MySqlServerVersion(new Version(8, 0)))); // Adjust version as needed

builder.Services.Configure<JsonOptions>(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

var app = builder.Build();

// Apply pending migrations and create the database if it doesn't exist
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FunctionsContext>();
    context.Database.Migrate();
}

RunMigration(app);

app.MapGet("/", () => "Healthy!");

app.MapGet("/api/functions", async (FunctionsContext db) =>
{
    var functions = await db.Functions.ToListAsync();
    return Results.Ok(functions);
});

app.MapGet("/api/functions/{id}", async (int id, FunctionsContext db) =>
    await db.Functions.FindAsync(id) is Function function
        ? Results.Ok(function)
        : Results.NotFound());

app.MapPost("/api/functions", async (Function function, FunctionsContext db) =>
{
    db.Functions.Add(function);
    await db.SaveChangesAsync();
    return Results.Created($"/api/functions/{function.Id}", function);
});

app.MapPut("/api/functions/{id}", async (int id, Function inputFunction, FunctionsContext db) =>
{
    var function = await db.Functions.FindAsync(id);
    if (function is null) return Results.NotFound();

    function.Name = inputFunction.Name;
    function.Image = inputFunction.Image;
    function.Language = inputFunction.Language;
    function.LanguageVersion = inputFunction.LanguageVersion;
    function.Cluster = inputFunction.Cluster;
    function.UserId = inputFunction.UserId;
    function.Status = inputFunction.Status;
    function.Message = inputFunction.Message;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapDelete("/api/functions/{id}", async (int id, FunctionsContext db) =>
{
    var function = await db.Functions.FindAsync(id);
    if (function is null) return Results.NotFound();

    db.Functions.Remove(function);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();

void RunMigration(WebApplication app)
{
  using (var scope = app.Services.CreateScope())
  {
    // Get the logger and FunctionsContext
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var context = scope.ServiceProvider.GetRequiredService<FunctionsContext>();

    try
    {
      logger.LogInformation("Starting database migration.");
      context.Database.Migrate();
      logger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
      logger.LogError(ex, "An error occurred while migrating the database.");
      throw; // Re-throw the exception to avoid silent failures
    }
  }
}

public partial class Program
{
}