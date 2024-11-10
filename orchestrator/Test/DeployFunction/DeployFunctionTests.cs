using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using FunctionsOrchestrator;
using FunctionsOrchestrator.Application.Services.BuildServices;
using FunctionsOrchestrator.Application.Services.DeployServices;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Test.DeployFunction
{
    public class BuildFunctionTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private HttpClient _client;

        public BuildFunctionTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(IGoBuilder));
                    if (descriptor != null) services.Remove(descriptor);
                    services.AddSingleton<IDeployer, StubDeployer>();
                });
            });

            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Forward_ReturnsOk_WhenResultIsSuccess()
        {
            // Arrange
            var content = new MultipartFormDataContent();
            content.Add(new StringContent("1.0"), "Version");
            content.Add(new StringContent("TestFunction"), "FunctionName");


            string directoryPath = "BuildFunction/GoExample"; // Replace with your directory path
            foreach (var filePath in Directory.GetFiles(directoryPath))
            {
                var fileContent = new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
                fileContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                content.Add(fileContent, "Files", Path.GetFileName(filePath));
            }

            // Act
            var response = await _client.PostAsync("/api/functions/build", content);

            // Assert
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
    }
}