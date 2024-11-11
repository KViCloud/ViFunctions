var builder = DistributedApplication.CreateBuilder(args);

var imageRegistry = builder.AddContainer("imageRegistry", "registry")
    .WithHttpEndpoint(targetPort: 5000, port:6200, name: "imageRegistry");

var goBuilder = builder.AddContainer("goBuilder", "quangnguyen2017/functionbuilder")
    .WithContainerRuntimeArgs("--privileged")
    .WithEnvironment("Services__Registry", "http://localhost:6200")
    .WithHttpEndpoint(targetPort: 8080, port:6001, name: "goBuilder");

var deployer = builder.AddGolangApp("deployer", "../../kernel/functiondeployer")
    .WithEnvironment("KUBECONFIG", Environment.GetEnvironmentVariable("KUBECONFIG"))
    .WithEnvironment("Services__Registry", "http://localhost:6200")
    .WithHttpEndpoint(targetPort: 8081, port:6101 ,name: "deployer");

var manager = builder.AddProject<Projects.ViFunction_Orchestrator>("manager")
    .WithEnvironment("Services__GoBuilderUrl", "http://localhost:6001")
    .WithEnvironment("Services__PythonBuilderUrl", "http://localhost:6002")
    .WithEnvironment("Services__DeployerUrl", "http://localhost:6101");

builder.Build().Run();