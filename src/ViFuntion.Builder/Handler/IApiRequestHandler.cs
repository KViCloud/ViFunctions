namespace ViFuntion.Builder.Handler;

public interface IApiRequestHandler
{
    Task<BuildResult> HandleApiRequest(HttpRequest request);
}