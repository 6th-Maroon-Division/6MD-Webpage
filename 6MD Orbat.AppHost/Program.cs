var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var DB = postgres.AddDatabase("6MDDatabase");

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects._6MD_Orbat_ApiService>("apiservice")
    .WithReference(DB)
    .WaitFor(DB);


builder.AddProject<Projects._6MD_Orbat_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
