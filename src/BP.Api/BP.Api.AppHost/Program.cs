var builder = DistributedApplication.CreateBuilder(args);

var storage = builder.AddAzureStorage("storage")
                     .RunAsEmulator(); // ensures Azurite is used locally

var tables = storage.AddTables("tables");
var blobs = storage.AddBlobs("blobs"); // Add blob storage resource

builder.AddProject<Projects.BP_Api>("bp-api")
    .WithEndpoint("http", e => e.Port = 5000)
    .WaitFor(tables)
    .WithReference(tables)
    .WithReference(blobs) // Reference blob storage in API
    .WithEnvironment("TableNames__Product", "Products")
    .WithEnvironment("TableNames__User", "Users")
    .WithEnvironment("BlobContainerNames__Images", "images"); // Set blob container name

builder.Build().Run();
