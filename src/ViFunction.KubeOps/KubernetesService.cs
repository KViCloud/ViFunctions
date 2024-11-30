using k8s;
using k8s.Models;
using Scriban;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace ViFunction.KubeOps;

public class KubernetesService
{
    private readonly IKubernetes _kClient;
    private readonly IDeserializer _deserializer;
    private readonly ILogger<KubernetesService> _logger;
    private const string HubNamespace = "funchub-ns";
    private const string KubeConfigPath = "Configs/kubeconfig";
    private const string DeploymentTemplate = "Templates/deployment.yaml";
    private const string HpaTemplate = "Templates/hpa.yaml";
    private const string ServiceTemplate = "Templates/service.yaml";

    public KubernetesService(ILogger<KubernetesService> logger)
    {
        _logger = logger;
        _kClient = BuildKubernetesClient(KubeConfigPath, logger);
        _deserializer = BuildDeserializer();
    }

    public async Task DeployAsync(DeploymentRequest request)
    {
        _logger.LogInformation("Starting deployment process for {Name}", request.Name);

        await CreateAndApplyResourceAsync<V1Deployment>(DeploymentTemplate, new
        {
            request.Name,
            request.Image,
            request.Replicas,
            request.CpuRequest,
            request.MemoryRequest,
            request.CpuLimit,
            request.MemoryLimit
        }, (client, resource, ns) => client.AppsV1.CreateNamespacedDeploymentAsync(resource, ns));

        await CreateAndApplyResourceAsync<V2HorizontalPodAutoscaler>(HpaTemplate, new
        {
            request.Name
        }, (client, resource, ns) => client.AutoscalingV2.CreateNamespacedHorizontalPodAutoscalerAsync(resource, ns));

        await CreateAndApplyResourceAsync<V1Service>(ServiceTemplate, new
        {
            request.Name
        }, (client, resource, ns) => client.CoreV1.CreateNamespacedServiceAsync(resource, ns));

        _logger.LogInformation("Deployment process for {Name} completed successfully", request.Name);
    }

    public async Task RollbackAsync(string resourceName)
    {
        _logger.LogInformation("Starting destroy process {Name}", resourceName);
        await DeleteResourceAsync((name, ns) => _kClient.AppsV1.DeleteNamespacedDeploymentAsync(name, ns), resourceName);
        await DeleteResourceAsync((name, ns) => _kClient.AutoscalingV2.DeleteNamespacedHorizontalPodAutoscalerAsync(name, ns), resourceName);
        await DeleteResourceAsync((name, ns) => _kClient.CoreV1.DeleteNamespacedServiceAsync(name, ns), resourceName);
        _logger.LogInformation("Destroy process for {Name} completed successfully", resourceName);
    }

    private IKubernetes BuildKubernetesClient(string kubeConfigPath, ILogger logger)
    {
        logger.LogInformation("Building Kubernetes client with kube config at {Path}", kubeConfigPath);
        var config = KubernetesClientConfiguration.BuildConfigFromConfigFile(kubeConfigPath);
        return new Kubernetes(config);
    }

    private IDeserializer BuildDeserializer()
    {
        return new DeserializerBuilder()
            .WithNamingConvention(CamelCaseNamingConvention.Instance)
            .Build();
    }

    private async Task<T> CreateAndApplyResourceAsync<T>(
        string templatePath, object data, Func<IKubernetes, T, string, Task> createFunc)
    {
        _logger.LogInformation("Creating and applying resource from template {TemplatePath}", templatePath);

        var templateContent = await File.ReadAllTextAsync(templatePath);
        var template = Template.Parse(templateContent);
        var renderedContent = template.Render(data);
        var resource = _deserializer.Deserialize<T>(renderedContent);

        await createFunc(_kClient, resource, HubNamespace);
        _logger.LogInformation("Resource created and applied successfully from template {TemplatePath}", templatePath);
        return resource;
    }

    private async Task DeleteResourceAsync(Func<string, string, Task> deleteFunc, string resourceName)
    {
        _logger.LogInformation("Deleting resource {ResourceName} in namespace {Namespace}", resourceName, HubNamespace);
        await deleteFunc(resourceName, HubNamespace);
        _logger.LogInformation("Resource {ResourceName} deleted successfully from namespace {Namespace}", resourceName, HubNamespace);
    }
}