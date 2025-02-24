var builder = DistributedApplication.CreateBuilder(args);


var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var DB = postgres.AddDatabase("MainDB");

var cache = builder.AddRedis("cache");

var apiService = builder.AddProject<Projects._6MD_ApiService>("apiservice")
    .WithReference(DB)
    .WaitFor(DB);


builder.AddProject<Projects._6MD_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);

builder.Build().Run();
