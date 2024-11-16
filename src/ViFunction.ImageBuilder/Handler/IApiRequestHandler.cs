namespace ViFunction.ImageBuilder.Handler;

public interface IApiRequestHandler
{
    Task<BuildResult> HandleApiRequest(HttpRequest request);
}