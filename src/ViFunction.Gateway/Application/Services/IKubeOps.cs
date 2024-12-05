using Refit;

namespace ViFunction.Gateway.Application.Services
{
    public interface IKubeOps
    {
        [Post("/deploy")]
        Task<IApiResponse<bool>> DeployAsync([Body] DeployDto request);

        [Post("/destroy")]
        Task<IApiResponse> RollbackAsync([Body] string name);
    }

    public record DeployDto(
        string Name,
        string Image,
        string CpuRequest,
        string MemoryRequest,
        string CpuLimit,
        string MemoryLimit
    );
}