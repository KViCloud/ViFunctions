namespace ViFunction.ImageBuilder.Handler;

public record BuildResult(
    bool Success,
    string Image,
    string Message);