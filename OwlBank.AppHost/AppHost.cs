

var builder = DistributedApplication.CreateBuilder(args);

var postgres = builder
    .AddPostgres("postgres")
    .WithDataVolume()
    .WithPgAdmin()
    .AddDatabase("OwlBankDb");

var auth = builder
    .AddProject("auth", @"..\OwlBank.Auth\OwlBank.Auth.csproj")
    .WithReference(postgres)
    .WithHttpEndpoint(
        port: 5002,
        targetPort: 5002,
        name: "http",
        isProxied: false)
    .WaitFor(postgres);

var api = builder
    .AddProject<Projects.OwlBank>("api")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WaitFor(auth);

var frontend = builder
    .AddExecutable(
        "frontend",
        "ng.cmd",
        "../../OwlBank-Fe",
        "serve")
    .WithReference(api)
    .WithHttpEndpoint(
        port: 4200,
        targetPort: 4200,
        name: "http",
        isProxied: false)
    .WaitFor(api);

builder.Build().Run();