using FunctionsManager.Extensions;

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

namespace FunctionsManager
{
    public partial class Program { }
}