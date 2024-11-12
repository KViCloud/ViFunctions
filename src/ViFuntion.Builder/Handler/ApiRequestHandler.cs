using System.Diagnostics;

namespace ViFuntion.Builder.Handler
{
    public record BuildResult(bool Success, string Message);

    public class ApiRequestHandler(ILogger<ApiRequestHandler> logger) : IApiRequestHandler
    {
        public async Task<BuildResult> HandleApiRequest(HttpRequest request)
        {
            var registryUrl = Environment.GetEnvironmentVariable("REGISTRY_URL");
            if (string.IsNullOrEmpty(registryUrl))
                return new BuildResult(false, "Docker registry URL is not configured.");

            logger.LogInformation("Handling API request.");

            var form = await request.ReadFormAsync();
            var files = form.Files;
            var imageName = form["imageName"].ToString();
            var version = form["version"].ToString();

            if (files.Count == 0 || string.IsNullOrEmpty(imageName))
                return new BuildResult(false, "Files and application name are required.");

            var tempPath = await StoreFilesInTempDirectory(imageName, files);

            // Build Image
            var language = DetectProgrammingLanguage(files);
            var containerfilePath = Path.Combine("ContainerTemplates", language, version, "Containerfile");
            if (!File.Exists(containerfilePath))
            {
                logger.LogError("Containerfile not found at path: {ContainerfilePath}", containerfilePath);
                return new BuildResult(false, "Containerfile not found.");
            }

            var buildahBuildCmd = $"buildah bud -f {containerfilePath} -t {imageName} {tempPath}";
            var built = RunCommand(buildahBuildCmd);
            if (!built)
                return new BuildResult(false, "Build image got an error.");

            // Push image

            var buildahPushCmd = $"buildah push {imageName} {registryUrl}/{imageName}:latest";
            var pushed = RunCommand(buildahPushCmd);
            if (!pushed)
                return new BuildResult(false, "Push had an error.");

            logger.LogInformation("Build and push successful for image: {ImageName}", imageName);
            return new BuildResult(true, "");
        }

        private async Task<string> StoreFilesInTempDirectory(string imageName, IFormFileCollection files)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), imageName);
            Directory.CreateDirectory(tempPath);
            logger.LogInformation("Created temporary directory: {TempPath}", tempPath);

            foreach (var file in files)
            {
                var filePath = Path.Combine(tempPath, file.FileName);
                await using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                logger.LogInformation("Saved file: {FilePath}", filePath);
            }

            return tempPath;
        }

        private string DetectProgrammingLanguage(IFormFileCollection files)
        {
            var language = "";
            if (files.Any(x => x.FileName.Contains(".go")))
                language = "Golang";
            else if (files.Any(x => x.FileName.Contains(".py")))
                language = "Python";
            return language;
        }

        private bool RunCommand(string command)
        {
            logger.LogInformation("Executing command: {Command}", command);

            var processStartInfo = new ProcessStartInfo
            {
                FileName = "/bin/bash",
                Arguments = $"-c \"{command}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using var process = new Process();
            process.StartInfo = processStartInfo;
            process.Start();

            var result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                var error = process.StandardError.ReadToEnd();
                logger.LogError("Command failed with error: {Error}", error);
                return false;
            }

            logger.LogInformation("Command executed successfully: {Result}", result);
            return true;
        }
    }
}