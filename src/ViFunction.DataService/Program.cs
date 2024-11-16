using System.Text.Json.Serialization; // Correct namespace for JSON options
using Microsoft.EntityFrameworkCore;
using ViFunction.DataService.Data;
using ViFunction.DataService.Models;
using Microsoft.AspNetCore.Http.Json; // Add this using directive

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

#if DEBUG
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<FunctionsContext>();
    context.Database.EnsureCreated();
}
#endif

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

// Run the app
app.Run();

public partial class Program
{
}