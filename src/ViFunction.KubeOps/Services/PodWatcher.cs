using k8s;
using k8s.Models;

namespace ViFunction.KubeOps.Services;

public class EventWatcher : BackgroundService
{
    private readonly ILogger<EventWatcher> _logger;
    private readonly IKubernetes _kubernetesClient;

    public EventWatcher(ILogger<EventWatcher> logger)
    {
        _logger = logger;
        _kubernetesClient = new Kubernetes(KubernetesClientConfiguration.InClusterConfig());
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                _logger.LogInformation("Starting to watch Kubernetes events...");

                // Create watcher for events across all namespaces
                var eventsListResp = await _kubernetesClient
                    .CoreV1.ListEventForAllNamespacesWithHttpMessagesAsync(
                        watch: true,
                        timeoutSeconds: 3600,
                        cancellationToken: stoppingToken
                    );

                using var eventWatcher = eventsListResp.Watch<Corev1Event, Corev1EventList>((type, item) =>
                {
                    var message = $"""
                                   Event Type: {type}
                                   Namespace: {item.Metadata.NamespaceProperty}
                                   Involved Object: {item.InvolvedObject.Kind}/{item.InvolvedObject.Name}
                                   Reason: {item.Reason}
                                   Message: {item.Message}
                                   First Timestamp: {item.FirstTimestamp}
                                   Last Timestamp: {item.LastTimestamp}
                                   Count: {item.Count}
                                   Type: {item.Type}
                                   Source: {item.Source.Component}/{item.Source.Host}
                                   """;

                    _logger.LogInformation(message);
                });

                // Keep the watcher running until cancellation is requested
                while (!stoppingToken.IsCancellationRequested)
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error watching Kubernetes events");

                // Wait before retrying
                await Task.Delay(5000, stoppingToken);
            }
        }
    }
}