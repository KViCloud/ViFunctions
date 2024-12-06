using System.Diagnostics;
using Microsoft.Extensions.Options;

namespace ViFunction.ImageBuilder.Handler
{
    public class ApiRequestHandler(ILogger<ApiRequestHandler> logger, IOptions<Registry> options) : IApiRequestHandler
    {
        private readonly Registry _registry = options.Value;

        public async Task<BuildResult> HandleApiRequest(HttpRequest request)
        {
            logger.LogInformation("Handling API request.");

            var form = await request.ReadFormAsync();
            var files = form.Files;
            var kname = form["kname"].ToString();
            var version = form["version"].ToString();
            const string defaultTag = "latest";

            if (files.Count == 0 || string.IsNullOrEmpty(kname))
                return new BuildResult(false, "", "Files and application name are required.");

            var tempPath = await StoreFilesInTempDirectory(kname, files);

            // Build Image
            var language = DetectProgrammingLanguage(files);
            var containerfilePath = Path.Combine("Sdk", language, version, "Containerfile");
            if (!File.Exists(containerfilePath))
            {
                logger.LogError("Containerfile not found at path: {ContainerfilePath}", containerfilePath);
                return new BuildResult(false, "", "Containerfile not found.");
            }

            var buildahBuildCmd = $"buildah bud -f {containerfilePath} -t {kname}:{defaultTag} {tempPath}";
            var built = RunCommand(buildahBuildCmd);
            if (!built)
                return new BuildResult(false,"", "Build image got an error.");

            // Login

            var buildahLoginCmd = $"buildah login -u {_registry.User} -p {_registry.Password} {_registry.BaseUrl}";
            var logged = RunCommand(buildahLoginCmd);
            if (!logged)
                return new BuildResult(false, "","Login got an error.");

            logger.LogInformation("Logged to: {Registry}", _registry.BaseUrl);

            // Push image
            var image = $"{_registry.BaseUrl}/{_registry.Path}/{kname}:{defaultTag}";
            var buildahPushCmd =
                $"buildah push {kname}:{defaultTag} {image}";
            var pushed = RunCommand(buildahPushCmd);
            if (!pushed)
                return new BuildResult(false, "","Push got an error.");

            logger.LogInformation("Build and push successful for image: {pushedImage}", image);
            return new BuildResult(true, image,"");
        }

        private async Task<string> StoreFilesInTempDirectory(string kname, IFormFileCollection files)
        {
            var tempPath = Path.Combine(Path.GetTempPath(), kname);
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
                FileName = "/bin/sh",
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