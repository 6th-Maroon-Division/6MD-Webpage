using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;
using OpenIddict.EntityFrameworkCore.Models;
using static OpenIddict.Abstractions.OpenIddictConstants;

var builder = WebApplication.CreateBuilder(args);

var DiscordOauthSecret = Environment.GetEnvironmentVariable("DiscordOauthSecret") ?? throw new Exception("DiscordOauthSecret not found!");
var DiscordOauthClientID = Environment.GetEnvironmentVariable("DiscordOauthClientID") ?? throw new Exception("DiscordOauthClientID not found!");

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
//builder.AddNpgsqlDbContext<DataStructure.DB>("MainDB");
builder.Services.AddDbContext<DataStructure.DB>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("MainDB"));
        options.UseOpenIddict();
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
#if DEBUG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endif

builder.Services.AddOpenIddict()
    .AddCore(options =>
    {
        options.UseEntityFrameworkCore()
               .UseDbContext<DataStructure.DB>();
    })
    .AddServer(options =>
    {
        options.SetTokenEndpointUris("/connect/token")
               .SetAuthorizationEndpointUris("/connect/authorize");

        options.AllowAuthorizationCodeFlow()
               .RequireProofKeyForCodeExchange(); // Enables PKCE

        options.RegisterScopes(Scopes.Email, Scopes.Profile, Scopes.OpenId, Scopes.Roles);

        // Register signing/encryption credentials
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Enable ASP.NET Core host integration
        options.UseAspNetCore()
               .EnableAuthorizationEndpointPassthrough()
               .EnableTokenEndpointPassthrough();
    })
    .AddClient(options =>
    {
        // Note: this sample only uses the authorization code flow,
        // but you can enable the other flows if necessary.
        options.AllowAuthorizationCodeFlow();

        // Register the signing and encryption credentials used to protect
        // sensitive data like the state tokens produced by OpenIddict.
        options.AddDevelopmentEncryptionCertificate()
               .AddDevelopmentSigningCertificate();

        // Register the ASP.NET Core host and configure the ASP.NET Core-specific options.
        options.UseAspNetCore()
               .EnableRedirectionEndpointPassthrough();

        // Register the System.Net.Http integration.
        options.UseSystemNetHttp();
        options.UseWebProviders()
                .AddDiscord(options =>
                {
                    options.SetClientId(DiscordOauthClientID);
                    options.SetClientSecret(DiscordOauthSecret);
                    options.SetRedirectUri("https://localhost:5001/auth/Discord/callback");
                });
    })
    .AddValidation(options =>
    {
        options.UseLocalServer();
        options.UseAspNetCore();
    });


var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseExceptionHandler();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
#if DEBUG
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "6MD API V1");
        options.RoutePrefix = string.Empty;
    });
#endif
}

{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<DataStructure.DB>();
    if (db.Database.GetPendingMigrations().Any())
        await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
