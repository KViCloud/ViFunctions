using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ViFunction.Storage.Data;

namespace ViFunction.Storage.Tests;

public class MemoryDbWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup> where TStartup : class
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove the existing DB context registration.
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<FunctionsContext>));
            if (descriptor != null) services.Remove(descriptor);

            // Add the in-memory database.
            services.AddDbContext<FunctionsContext>(options =>
            {
                options.UseInMemoryDatabase("InMemoryDbForTesting");
            });

            // Build the service provider.
            var serviceProvider = services.BuildServiceProvider();
            
            // Create the scope for seeding the database with test data.
            using var scope = serviceProvider.CreateScope();
            var scopedServices = scope.ServiceProvider;

            var db = scopedServices.GetRequiredService<FunctionsContext>();
            db.Database.EnsureCreated();
            
            // Seed the database with test data
            SeedTestData(db);
        });

        return base.CreateHost(builder);
    }

    private void SeedTestData(FunctionsContext context)
    {
        context.Functions.Add(new ViFunction.Storage.Models.Function
        {
            Id = 1,
            Name = "Test Function",
            Image = "example/testimage.png",
            Language = "C#",
            LanguageVersion = "9.0",
            Cluster = "TestCluster",
            UserId = "TestUser"
        });

        context.SaveChanges();
    }
}