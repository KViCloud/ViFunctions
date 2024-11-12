using System.Net.Http.Json;
using ViFunction.Repository.Models;

namespace ViFunction.Repository.Tests;

public class FunctionsApiTests : IClassFixture<MemoryDbWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FunctionsApiTests(MemoryDbWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetById_ShouldSucceed()
    {
        var response = await _client.GetAsync("/api/functions/1");
        response.EnsureSuccessStatusCode();

        var function = await response.Content.ReadFromJsonAsync<Function>();
        Assert.NotNull(function);
        Assert.Equal(1, function.Id);
    }

    [Fact]
    public async Task CreatesFunction_ShouldReturnCreated()
    {
        var newFunction = new Function
        {
            Name = "New Function",
            Image = "example/newimage.png",
            Language = "Python",
            LanguageVersion = "3.9",
            Cluster = "NewCluster",
            UserId = "NewUser"
        };

        var response = await _client.PostAsJsonAsync("/api/functions", newFunction);
        response.EnsureSuccessStatusCode();

        var createdFunction = await response.Content.ReadFromJsonAsync<Function>();
        Assert.NotNull(createdFunction);
        Assert.Equal("New Function", createdFunction.Name);
    }

    [Fact]
    public async Task UpdatesFunction_ShouldSucceed()
    {
        var updatedFunction = new Function
        {
            Name = "Updated Function",
            Image = "example/updatedimage.png",
            Language = "JavaScript",
            LanguageVersion = "ES6",
            Cluster = "UpdatedCluster",
            UserId = "UpdatedUser"
        };

        var response = await _client.PutAsJsonAsync("/api/functions/1", updatedFunction);
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/api/functions/1");
        var function = await getResponse.Content.ReadFromJsonAsync<Function>();
        Assert.NotNull(function);
        Assert.Equal("Updated Function", function.Name);
    }

    [Fact]
    public async Task DeleteFunction_ShouldSuccess()
    {
        var response = await _client.DeleteAsync("/api/functions/1");
        response.EnsureSuccessStatusCode();

        var getResponse = await _client.GetAsync("/api/functions/1");
        Assert.Equal(System.Net.HttpStatusCode.NotFound, getResponse.StatusCode);
    }
}