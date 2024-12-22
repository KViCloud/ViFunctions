using k8s;
using k8s.Models;

namespace ViFunction.KubeOps.Services;

public class Operation: IOperation
{
    private readonly Kubernetes _kClient;
    private readonly ILogger<Operation> _logger;
    private const string HubNamespace = "funchub-ns";

    public Operation(ILogger<Operation> logger)
    {
        _logger = logger;
        _kClient = new Kubernetes(KubernetesClientConfiguration.InClusterConfig());
    }

    public async Task<bool> DeployAsync(DeploymentRequest request)
    {
        _logger.LogInformation("Starting deployment process for {Name}", request.Name);
        try
        {
            await CreateDeployment(request);
            await CreateService(request);
            await CreateHorizontalPodAutoscaler(request);
            _logger.LogInformation("Deployment process for {Name} completed successfully", request.Name);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the deployment process for {Name}: {ExceptionMessage}",
                request.Name, ex.Message);
            _logger.LogInformation("Deploy {Name} failed, rollback now", request.Name);
            await DestroyAsync(request.Name);
            return false;
        }
    }

    public async Task DestroyAsync(string functionName)
    {
        _logger.LogInformation("Starting destroy {functionName}", functionName);
        var v1DeploymentList = await _kClient.ListNamespacedDeploymentAsync(
            namespaceParameter: HubNamespace, 
            fieldSelector: $"metadata.name={functionName}-deployment"
            );
        _logger.LogInformation($"Count {v1DeploymentList.Items.Count()} Deployment {functionName} deleting.");

        if (v1DeploymentList.Items.Any())
        {
            _logger.LogInformation($"Deployment {functionName} deleting.");
            await _kClient.DeleteNamespacedDeploymentAsync($"{functionName}-deployment", HubNamespace);
            _logger.LogInformation($"Deployment {functionName} deleted successfully.");
        }

        var v2HorizontalPodAutoscalerList = await _kClient.AutoscalingV2.ListNamespacedHorizontalPodAutoscalerAsync(
            namespaceParameter: HubNamespace, 
            fieldSelector: $"metadata.name={functionName}-hpa"
        );

        if (v2HorizontalPodAutoscalerList.Items.Any())
        {
            _logger.LogInformation($"Count {v2HorizontalPodAutoscalerList.Items.Count} Hpa {functionName} deleting.");
            await _kClient.AutoscalingV2.DeleteNamespacedHorizontalPodAutoscalerAsync($"{functionName}-hpa", HubNamespace);
            _logger.LogInformation($"Hpa {functionName} deleted successfully.");
        }

        var v1ServiceList = await _kClient.ListNamespacedServiceAsync(
            namespaceParameter: HubNamespace,
            fieldSelector: $"metadata.name={functionName}-service"
            );

        if (v1ServiceList.Items.Any())
        {
            _logger.LogInformation($"Count {v1ServiceList.Items.Count} Service {functionName} deleting.");
            await _kClient.DeleteNamespacedServiceAsync($"{functionName}-service", HubNamespace);
            _logger.LogInformation($"Service {functionName} deleted successfully.");
        }

        _logger.LogInformation("Destroy process for {functionName} completed successfully", functionName);
    }

    private async Task CreateDeployment(DeploymentRequest request)
    {
        var deployment = new V1Deployment
        {
            Metadata = new V1ObjectMeta
            {
                Name = $"{request.Name}-deployment",
                NamespaceProperty = HubNamespace
            },
            Spec = new V1DeploymentSpec
            {
                Selector = new V1LabelSelector
                {
                    MatchLabels = new Dictionary<string, string>
                    {
                        { "app", $"{request.Name}" }
                    }
                },
                Replicas = 1,
                Template = new V1PodTemplateSpec
                {
                    Metadata = new V1ObjectMeta
                    {
                        Labels = new Dictionary<string, string>
                        {
                            { "app", $"{request.Name}" }
                        }
                    },
                    Spec = new V1PodSpec
                    {
                        Containers =
                        [
                            new V1Container
                            {
                                Name = $"{request.Name}",
                                Image = $"{request.Image}",
                                Ports =
                                [
                                    new V1ContainerPort(8080)
                                ],
                                Resources = new V1ResourceRequirements
                                {
                                    Requests = new Dictionary<string, ResourceQuantity>
                                    {
                                        { "cpu", new ResourceQuantity(request.CpuRequest) },
                                        { "memory", new ResourceQuantity(request.MemoryRequest) }
                                    },
                                    Limits = new Dictionary<string, ResourceQuantity>
                                    {
                                        { "cpu", new ResourceQuantity(request.CpuLimit) },
                                        { "memory", new ResourceQuantity(request.MemoryLimit) }
                                    }
                                }
                            }
                        ],
                    }
                }
            }
        };
        var createdDeployment = await _kClient.CreateNamespacedDeploymentAsync(deployment, HubNamespace);
        Console.WriteLine($"Deployment {createdDeployment.Metadata.Name} created successfully.");
    }

    private async Task CreateService(DeploymentRequest request)
    {
        var service = new V1Service
        {
            Metadata = new V1ObjectMeta
            {
                Name = $"{request.Name}-service",
                NamespaceProperty = HubNamespace
            },
            Spec = new V1ServiceSpec
            {
                Selector = new Dictionary<string, string>
                {
                    { "app", $"{request.Name}" }
                },
                Ports =
                [
                    new V1ServicePort
                    {
                        Port = 8080,
                        TargetPort = 8080
                    }
                ],
                Type = "ClusterIP"
            }
        };

        var createdService = await _kClient.CreateNamespacedServiceAsync(service, HubNamespace);
        Console.WriteLine($"Service {createdService.Metadata.Name} created successfully.");
    }

    private async Task CreateHorizontalPodAutoscaler(DeploymentRequest request)
    {
        var hpa = new V2HorizontalPodAutoscaler
        {
            Metadata = new V1ObjectMeta
            {
                Name = $"{request.Name}-hpa",
                NamespaceProperty = HubNamespace
            },
            Spec = new V2HorizontalPodAutoscalerSpec
            {
                ScaleTargetRef = new V2CrossVersionObjectReference
                {
                    ApiVersion = "apps/v1",
                    Kind = "Deployment",
                    Name = $"{request.Name}-deployment"
                },
                MinReplicas = 1,
                MaxReplicas = 10,
                Metrics =
                [
                    new V2MetricSpec
                    {
                        Type = "Resource",
                        Resource = new V2ResourceMetricSource
                        {
                            Name = "cpu",
                            Target = new V2MetricTarget
                            {
                                Type = "Utilization",
                                AverageUtilization = 70
                            }
                        }
                    },
                    new V2MetricSpec
                    {
                        Type = "Resource",
                        Resource = new V2ResourceMetricSource
                        {
                            Name = "memory",
                            Target = new V2MetricTarget
                            {
                                Type = "Utilization",
                                AverageUtilization = 70 // Example threshold for memory utilization
                            }
                        }
                    }
                ]
            }
        };

        var createdHpa = await _kClient.CreateNamespacedHorizontalPodAutoscalerAsync(hpa, HubNamespace);
        Console.WriteLine($"Horizontal Pod Autoscaler {createdHpa.Metadata.Name} created successfully.");
    }

}