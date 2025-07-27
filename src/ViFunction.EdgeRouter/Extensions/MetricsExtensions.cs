using Prometheus;

namespace ViFunction.EdgeRouter.Extensions;

public static class MetricsExtensions
{
    private static readonly Counter RequestCounter = Metrics.CreateCounter(
        "edge_router_requests_total",
        "Total requests routed through the edge router",
        new CounterConfiguration
        {
            LabelNames = new[] { "method", "status_code", "function_name" }
        });

    public static IApplicationBuilder UseEdgeRouterMetrics(this IApplicationBuilder app)
    {
        app.UseMetricServer();
        app.UseHttpMetrics();

        app.Use(async (context, next) =>
        {
            var method = context.Request.Method;
            var functionName = context.Request.RouteValues.TryGetValue("functionName", out var fn)
                ? fn?.ToString() ?? "unknown"
                : "unknown";

            await next();

            var statusCode = context.Response.StatusCode.ToString();

            RequestCounter
                .WithLabels(method, statusCode, functionName)
                .Inc();
        });

        return app;
    }
}