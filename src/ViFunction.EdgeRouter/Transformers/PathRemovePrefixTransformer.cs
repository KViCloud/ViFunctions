using Yarp.ReverseProxy.Forwarder;
namespace ViFunction.EdgeRouter.Transformers;

public class PathRemovePrefixTransformer : HttpTransformer
{
    private readonly string _prefixToRemove;

    public PathRemovePrefixTransformer(string prefixToRemove)
    {
        _prefixToRemove = prefixToRemove;
    }

    public override ValueTask TransformRequestAsync(HttpContext context, HttpRequestMessage proxyRequest, string destinationPrefix)
    {
        var originalPath = context.Request.Path;
        var pathToForward = originalPath.Value!.Substring(_prefixToRemove.Length);

        proxyRequest.RequestUri = RequestUtilities.MakeDestinationAddress(
            destinationPrefix,
            pathToForward,
            context.Request.QueryString);

        proxyRequest.Headers.Host = null;
        return ValueTask.CompletedTask;
    }
}