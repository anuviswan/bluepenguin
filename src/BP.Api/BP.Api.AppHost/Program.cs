var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator(); // ensures Azurite is used locally

var tables = storage.AddTables("tables");
builder.AddProject<Projects.BP_Api>("bp-api")
    .WaitFor(tables)
    .WithReference(tables);

builder.Build().Run();
