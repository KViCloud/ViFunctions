using ViFunction.Orchestrator.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterAppServices();

builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();

namespace ViFunction.Orchestrator
{
    public partial class Program { }
}