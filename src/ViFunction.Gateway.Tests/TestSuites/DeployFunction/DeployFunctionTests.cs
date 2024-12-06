using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ViFunction.Gateway.Application.Commands;
using ViFunction.Gateway.Tests.Utils;

namespace ViFunction.Gateway.Tests.TestSuites.DeployFunction
{
    public class BuildFunctionTests(StubWebApplicationFactory<Program> factory)
        : IClassFixture<StubWebApplicationFactory<Program>>
    {
        private HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task Forward_ReturnsOk_WhenResultIsSuccess()
        {
            // Arrange
            var command = new DeployCommand
            {
                FunctionId = Guid.NewGuid() // Adjust as per your setup
            };

            // Act
            var response = await _client.PostAsJsonAsync("/api/functions/deploy", command);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}