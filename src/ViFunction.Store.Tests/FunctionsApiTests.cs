using System.Net.Http.Json;
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
        var functions = await responseAll.Content.ReadFromJsonAsync<List<FunctionDto>>();

        var response = await _client.GetAsync($"/api/functions/{functions!.First().Id}");
        response.EnsureSuccessStatusCode();
        var function = await response.Content.ReadFromJsonAsync<FunctionDto>();
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

        var createdFunction = await response.Content.ReadFromJsonAsync<FunctionDto>();
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
        var function = await response.Content.ReadFromJsonAsync<FunctionDto>();

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
        function = await response.Content.ReadFromJsonAsync<FunctionDto>();
        Assert.NotNull(function);
        Assert.Equal(FunctionStatus.Built.ToString(), function.FunctionStatus);
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
        var function = await response.Content.ReadFromJsonAsync<FunctionDto>();

        //Act
        var getResponse = await _client.DeleteAsync($"/api/functions/{function!.Id}");
        //Assert
        Assert.Equal(System.Net.HttpStatusCode.OK, getResponse.StatusCode);
    }
}