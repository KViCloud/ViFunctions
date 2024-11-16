using System.Net;
using System.Net.Http.Headers;
using FluentAssertions;
using ViFunction.Gateway.Tests.Utils;

namespace ViFunction.Gateway.Tests.InitFunction
{
    public class BuildFunctionTests(StubWebApplicationFactory<Program> factory)
        : IClassFixture<StubWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client = factory.CreateClient();

        [Fact]
        public async Task InitFunctionApp_ShouldSuccess()
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