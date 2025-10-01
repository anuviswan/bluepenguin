var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator(); // ensures Azurite is used locally

var tables = storage.AddTables("tables");
builder.AddProject<Projects.BP_Api>("bp-api")
    .WithEndpoint("http", e => e.Port = 5000)
    .WaitFor(tables)
    .WithReference(tables)
    .WithEnvironment("TableNames__Product", "Products")
    .WithEnvironment("TableNames__User", "Users");

builder.Build().Run();
