var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("OwlBankDb");

var api = builder
    .AddProject<Projects.OwlBank>("api")
    .WithReference(postgres)
    .WaitFor(postgres);

var frontend = builder
    .AddExecutable(
        "frontend",
        "npm.cmd",
        "../OwlBank-Fe",
        "start")
    .WithReference(api)                  // injects services__api__http__0 / https__0
    .WithHttpEndpoint(
        port: 4200,
        targetPort: 4200,
        name: "http",
        isProxied: false)                // no Aspire proxy, so Host stays localhost:4200
    .WaitFor(api);

builder.Build().Run();