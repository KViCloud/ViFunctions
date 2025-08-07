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

            logger.LogInformation("=== Incoming Request ===");
            logger.LogInformation("Method: {Method}", context.Request.Method);
            logger.LogInformation("Scheme: {Scheme}", context.Request.Scheme);
            logger.LogInformation("Host: {Host}", context.Request.Host);
            logger.LogInformation("Path: {Path}", context.Request.Path);
            logger.LogInformation("QueryString: {QueryString}", context.Request.QueryString);

            var clientIp = context.Request.Headers["X-Forwarded-For"].FirstOrDefault()
                           ?? context.Connection.RemoteIpAddress?.ToString();
            var userAgent = context.Request.Headers["User-Agent"].ToString();

            logger.LogInformation("Client IP: {ClientIp}", clientIp ?? "Unknown");
            logger.LogInformation("User-Agent: {UserAgent}", userAgent ?? "Unknown");

            logger.LogInformation("Headers:");
            foreach (var header in context.Request.Headers)
            {
                logger.LogInformation("  {Key}: {Value}", header.Key, string.Join(", ", header.Value.ToString()));
            }

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