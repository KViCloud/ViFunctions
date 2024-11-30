var builder = DistributedApplication.CreateBuilder(args);

var mysql = builder.AddMySql("mysql");
var functionDb = mysql.AddDatabase("FunctionsDatabase");
var store = builder.AddProject<Projects.ViFunction_Store>("store")
    .WithReference(functionDb)
    .WaitFor(functionDb)
    .WithHttpEndpoint(port: 6001, name: "dataEndpoint");

var certsPath = Path.Combine("/Users/quang/Documents/GitHub/ViFunctions/src/ViFunction.AppHost", "certs");

var registryService = builder.AddContainer("registry", "registry", "2")
    .WithEndpoint(targetPort: 5000, port: 6002);

registryService.GetEndpoint("registryEndpoint");

var builderService = builder.AddDockerfile(
        "builderservice", "../ViFunction.ImageBuilder", "../ViFunction.ImageBuilder/Containerfile")
        .WithContainerRuntimeArgs("--privileged")
    .WithReference(registryService.GetEndpoint("registryEndpoint"))
    .WithEnvironment("Services__ImageRegistry", "localhost:6002")
    .WithHttpEndpoint(targetPort: 8080, port: 6003, name: "imagebuilder");

var gateway = builder.AddProject<Projects.ViFunction_Gateway>("gateway")
    .WithEnvironment("Services__StoreUrl", "http://localhost:6001")
    .WithEnvironment("Services__BuilderUrl", "http://localhost:6003")
    .WithEnvironment("Services__DeployerUrl", "http://localhost:6101")
    .WithHttpsEndpoint(targetPort: 8082, port: 6004, name: "gatewayEndpoint");
;

builder.Build().Run();