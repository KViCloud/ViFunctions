using System.ComponentModel.DataAnnotations;

namespace ViFunction.Store.Application.Entities;

public class Function
{
    public Guid Id { get; private set; }
    [MaxLength(100)] public string Name { get; private set; }
    [MaxLength(200)] public string Image { get; private set; }
    [MaxLength(50)] public string Language { get; private set; }
    [MaxLength(10)] public string LanguageVersion { get; private set; }
    [MaxLength(100)] public string Cluster { get; private set; }
    [MaxLength(50)] public string UserId { get; private set; }
    [MaxLength(50)] public FunctionStatus Status { get; private set; }
    [MaxLength(500)] public string Message { get; private set; }
    [MaxLength(100)] public string KubernetesName { get; private set; }
    [MaxLength(50)] public string Tier { get; set; }

    [MaxLength(10)] public string CpuRequest { get; private set; }
    [MaxLength(10)] public string MemoryRequest { get; private set; }
    [MaxLength(10)] public string CpuLimit { get; private set; }
    [MaxLength(10)] public string MemoryLimit { get; private set; }
    

    public Function(
        string name,
        string image,
        string language,
        string languageVersion,
        string userId,
        string kubernetesName)
    {
        Id = Guid.NewGuid();
        Name = name;
        Image = image;
        Language = language;
        LanguageVersion = languageVersion;
        UserId = userId;
        Status = FunctionStatus.None;
        Message = "Created";
        KubernetesName = kubernetesName;
    }
    
    
    
    public void SetCluster(string cluster)
    {
        Cluster = cluster;
    }
    public void SetResource(
        string tier,
        string cpuRequest,
        string memoryRequest,
        string cpuLimit,
        string memoryLimit)
    {
        Tier = tier;
        CpuRequest = cpuRequest;
        MemoryRequest = memoryRequest;
        CpuLimit = cpuLimit;
        MemoryLimit = memoryLimit;
    }


    public void SetStatus(
        FunctionStatus functionStatus,
        string message)
    {
        Status = functionStatus;
        Message = message;
    }
    
    public void SetImage(string image)
    {
        Status = FunctionStatus.Built;
        Image = image;
    }
}
public enum FunctionStatus
{
    None,
    Built,
    Deployed,
    Destroyed,
}