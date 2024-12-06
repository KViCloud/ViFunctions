using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using ViFunction.Store.Application.Dtos;
using ViFunction.Store.Application.Entities;
using ViFunction.Store.Application.Requests;

namespace ViFunction.Store.Tests;

public class FunctionsApiTests : IClassFixture<MemoryDbWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FunctionsApiTests(MemoryDbWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_ShouldSucceed()
    {
        var responseAll = await _client.GetAsync($"/api/functions/");
        responseAll.EnsureSuccessStatusCode();
        var functions = await responseAll.Content.ReadResponseAsync<List<FunctionDto>>();

        var response = await _client.GetAsync($"/api/functions/{functions!.First().Id}");
        response.EnsureSuccessStatusCode();
        var function = await response.Content.ReadResponseAsync<FunctionDto>();
        Assert.NotNull(function);
    }

    [Fact]
    public async Task CreatesFunction_ShouldReturnCreated()
    {
        //Act
        var command = new CreateFunctionCommand
        {
            Name = "New Function",
            Language = "Python",
            LanguageVersion = "3.9",
            UserId = "NewUser"
        };

        var response = await _client.PostAsJsonAsync("/api/functions", command);

        //Assert
        response.EnsureSuccessStatusCode();

        var createdFunction = await response.Content.ReadResponseAsync<FunctionDto>();
        Assert.NotNull(createdFunction);
        Assert.Equal("New Function", createdFunction.Name);
    }

    [Fact]
    public async Task UpdatesFunction_ShouldSucceed()
    {
        //Arrange
        var command = new CreateFunctionCommand
        {
            Name = "New Function",
            Language = "Python",
            LanguageVersion = "3.9",
            UserId = "NewUser"
        };
        var response = await _client.PostAsJsonAsync("/api/functions", command);
        response.EnsureSuccessStatusCode();
        var function = await response.Content.ReadResponseAsync<FunctionDto>();

        //Act
        response = await _client.PutAsJsonAsync($"/api/functions/{function.Id}", new UpdateFunctionCommand()
        {
            Id = function.Id,
            Status = FunctionStatus.Built,
            Message = "Function built successfully"
        });

        //Assert
        response.EnsureSuccessStatusCode();
        response = await _client.GetAsync($"/api/functions/{function.Id}");

        function = await response.Content.ReadResponseAsync<FunctionDto>();
        Assert.NotNull(function);
        Assert.Equal(FunctionStatus.Built, function.Status);
    }

    [Fact]
    public async Task DeleteFunction_ShouldSuccess()
    {
        //Arrange
        var command = new CreateFunctionCommand
        {
            Name = "New Function",
            Language = "Python",
            LanguageVersion = "3.9",
            UserId = "NewUser"
        };
        var response = await _client.PostAsJsonAsync("/api/functions", command);
        response.EnsureSuccessStatusCode();
        var function = await response.Content.ReadResponseAsync<FunctionDto>();

        //Act
        var getResponse = await _client.DeleteAsync($"/api/functions/{function!.Id}");
        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
    }
}

public static class JsonExtensions
{
    public static async Task<T> ReadResponseAsync<T>(this HttpContent content)
    {
        var jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
        var json = await content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(json, jsonOptions);
    }
}