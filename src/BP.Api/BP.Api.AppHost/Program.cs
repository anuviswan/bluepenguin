var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.BP_Api>("bp-api");

builder.Build().Run();
