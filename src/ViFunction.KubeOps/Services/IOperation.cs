namespace ViFunction.KubeOps.Services;

public interface IOperation
{
    Task<bool> DeployAsync(DeploymentRequest request);
    Task DestroyAsync(string functionName);
}