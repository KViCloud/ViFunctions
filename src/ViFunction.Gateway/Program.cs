using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using ViFunction.Gateway.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.RegisterAppServices();

builder.Services.AddControllers()
  .AddJsonOptions(options =>
  {
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
  });

builder.AddServiceDefaults();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();app.UseRouting();

app.MapControllers();

app.MapGet("/", () => "Healthy!");

app.Run();

namespace ViFunction.Gateway
{
  public partial class Program
  {
  }
}