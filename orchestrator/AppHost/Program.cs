var builder = DistributedApplication.CreateBuilder(args);

var goBuilder = builder.AddContainer("goBuilder", "quangnguyen2017/functionbuilder")
    .WithContainerRuntimeArgs("--privileged")
    .WithEnvironment("DOCKER_USERNAME", Environment.GetEnvironmentVariable("DOCKER_USERNAME"))
    .WithEnvironment("DOCKER_PASSWORD", Environment.GetEnvironmentVariable("DOCKER_PASSWORD"))
    .WithHttpEndpoint(targetPort: 8080, port:6001, name: "goBuilder");

var deployer = builder.AddGolangApp("deployer", "../../kernel/functiondeployer")
    .WithEnvironment("DOCKER_PASSWORD", Environment.GetEnvironmentVariable("KUBECONFIG"))
    .WithHttpEndpoint(targetPort: 8081, port:6101 ,name: "deployer");

var manager = builder.AddProject<Projects.FunctionsManager>("manager")
    .WithEnvironment("Services__GoBuilderUrl", "http://localhost:6001")
    .WithEnvironment("Services__PythonBuilderUrl", "http://localhost:6002")
    .WithEnvironment("Services__DeployerUrl", "http://localhost:6101");

builder.Build().Run();