var builder = DistributedApplication.CreateBuilder(args);

builder.AddProject<Projects.WebApplication1>("dptest")
    .WithExternalHttpEndpoints();

builder.Build().Run();
