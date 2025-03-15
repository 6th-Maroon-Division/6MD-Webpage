using Projects;

var builder = DistributedApplication.CreateBuilder(args);

var DiscordOauthSecret = Environment.GetEnvironmentVariable("DiscordOauthSecret") ?? throw new Exception("DiscordOauthSecret not found!");
var DiscordOauthClientID = Environment.GetEnvironmentVariable("DiscordOauthClientID") ?? throw new Exception("DiscordOauthClientID not found!");

var postgres = builder.AddPostgres("postgres")
    .WithPgAdmin();

var DB = postgres.AddDatabase("MainDB");

var UserDB = postgres.AddDatabase("UserDB");
var OperationDB = postgres.AddDatabase("OperationDB");

var cache = builder.AddRedis("cache");

var authServer = builder.AddProject<Projects._6MD_AuthServer>("Auth-Server")
    .WithReference(UserDB)
    .WaitFor(UserDB)
    .WithEnvironment("DiscordOauthClientID", DiscordOauthClientID)
    .WithEnvironment("DiscordOauthSecret", DiscordOauthSecret);

var apiService = builder.AddProject<_6MD_ApiService>("apiservice")
    .WithReference(DB)
    .WaitFor(DB)
    .WithReference(authServer)
    .WaitFor(authServer);

/*builder.AddProject<Projects._6MD_Webold>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService);*/

builder.AddProject<_6MD_Web>("webfrontend")
    .WithExternalHttpEndpoints()
    .WithReference(cache)
    .WaitFor(cache)
    .WithReference(apiService)
    .WaitFor(apiService)
    .WithReference(authServer);


builder.Build().Run();
