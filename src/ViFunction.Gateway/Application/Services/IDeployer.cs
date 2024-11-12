using Refit;

namespace ViFunction.Gateway.Application.Services;

public class DeployDto(string name, string image)
{
    public string Name { get; private set; } = name;
    public string Image { get; private set; } = image;
}
public interface IDeployer
{
    [Post("/deploy")]
    Task<IApiResponse> DeployAsync(DeployDto deployDto);
}