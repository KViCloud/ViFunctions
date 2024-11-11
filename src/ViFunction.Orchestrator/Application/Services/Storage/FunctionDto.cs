using System.ComponentModel.DataAnnotations;

namespace ViFunction.Orchestrator.Application.Services.Storage;

public class FunctionDto
{
    public int Id { get; set; }
    [MaxLength(100)] public string Name { get; set; }
    [MaxLength(200)] public string Image { get; set; }
    [MaxLength(50)] public string Language { get; set; }
    [MaxLength(10)] public string LanguageVersion { get; set; }
    [MaxLength(100)] public string Cluster { get; set; }
    [MaxLength(50)] public string UserId { get; set; }
}