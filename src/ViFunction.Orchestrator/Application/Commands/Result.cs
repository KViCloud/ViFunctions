namespace ViFunction.Orchestrator.Application.Commands;

public class Result(bool isSuccess = true, string description = "")
{
    public bool IsSuccess { get; } = isSuccess;
    public string Description { get; } = description;
}