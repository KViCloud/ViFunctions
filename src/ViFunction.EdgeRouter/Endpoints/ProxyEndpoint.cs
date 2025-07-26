using ViFunction.EdgeRouter.Transformers;
using Yarp.ReverseProxy.Forwarder;

namespace ViFunction.EdgeRouter.Endpoints;

public static class ProxyEndpoint
{
    public static void MapProxy(this WebApplication app)
    {
        app.Map("/{prefix}/{functionName}/{**rest}", async (
            HttpContext context,
            IHttpForwarder forwarder,
            HttpMessageInvoker httpClient,
            string prefix,
            string functionName
        ) =>
        {
            var logger = app.Logger;

            logger.LogInformation("Proxying request:");
            logger.LogInformation("Method: {Method}", context.Request.Method);
            logger.LogInformation("Path: {Path}", context.Request.Path);

            if (!string.Equals(prefix, "function", StringComparison.OrdinalIgnoreCase))
                return Results.NotFound();

            var targetNamespace = "funchub-ns";
            var targetPort = "8080";
            var targetUri = new Uri($"http://{functionName}-service.{targetNamespace}.svc.cluster.local:{targetPort}");

            var requestOptions = new ForwarderRequestConfig
            {
                ActivityTimeout = TimeSpan.FromSeconds(60),
            };

            var transformer = new PathRemovePrefixTransformer($"/{prefix}/{functionName}");

            var error = await forwarder.SendAsync(context, targetUri.ToString(), httpClient, requestOptions,
                transformer);

            if (error != ForwarderError.None)
            {
                var errorFeature = context.GetForwarderErrorFeature();
                context.Response.StatusCode = 404;
                await context.Response.WriteAsync("Function not found.");
                logger.LogError($"Proxying failed: {errorFeature?.Exception?.Message}");
            }

            return Results.Empty;
        });
    }
}