using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ViFunction.Gateway.Application.Commands;
using ViFunction.Gateway.Application.Services;
using ViFunction.Gateway.Tests.Utils;

namespace ViFunction.Gateway.Tests.TestSuites.InitFunction
{
    public class BuildFunctionTests(StubWebApplicationFactory<Program> factory)
        : IClassFixture<StubWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task InitFunctionApp_ShouldSuccess()
        {
            // Arrange
            var command = new InitCommand
            {
                Language = "C#",
                Version = "9.0",
                FunctionName = "TestFunction"
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/functions/init", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        
        [Fact]
        public async Task GetAll_ReturnsOkAndValidContent()
        {
            // Act
            var response = await _client.GetAsync("/api/functions");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var functions = await response.Content.ReadFromJsonAsync<List<FunctionDto>>();
            Assert.NotNull(functions);
        }
    }
}