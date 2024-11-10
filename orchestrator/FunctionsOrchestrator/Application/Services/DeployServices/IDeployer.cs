using System.Text.Json.Serialization;
using Refit;

namespace FunctionsOrchestrator.Application.Services.DeployServices;

public class DeployParams(string name, string image)
{
    [JsonPropertyName("name")]
    public string Name { get; private set; } = name;
    [JsonPropertyName("image")]
    public string Image { get; private set; } = image;
}

public interface IDeployer
{
    [Post("/deploy")]
    Task<IApiResponse> DeployAsync(DeployParams deployParams);
}