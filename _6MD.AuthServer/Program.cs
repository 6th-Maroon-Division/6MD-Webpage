
using _6MD.AuthServer.services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using static OpenIddict.Server.OpenIddictServerEvents;

namespace _6MD.AuthServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        (string clientid, string secret) DiscordOauth = 
            (Environment.GetEnvironmentVariable("DiscordOauthClientID") ?? throw new Exception("DiscordOauthClientID not found!"),
            Environment.GetEnvironmentVariable("DiscordOauthSecret") ?? throw new Exception("DiscordOauthSecret not found!"));


        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.
        var connectionString = builder.Configuration.GetConnectionString("UserDB");
        builder.Services.AddDbContext<DB.UserDB>(options =>
        {
            options.UseNpgsql(connectionString);
            options.UseOpenIddict();
        }
            );



        builder.Services.AddOpenIddict()

        // Register the OpenIddict core components.
        .AddCore(options =>
        {
            // Configure OpenIddict to use the Entity Framework Core stores and models.
            // Note: call ReplaceDefaultEntities() to replace the default entities.
            options.UseEntityFrameworkCore()
                   .UseDbContext<DB.UserDB>();
            })
        .AddServer(options =>
        {
            // Enable the token endpoint.
            options.SetTokenEndpointUris("/connect/token");
            //options.SetAuthorizationEndpointUris("auth/Authorizatzion/auth");
            options.SetIntrospectionEndpointUris("/connect/introspect");

            // Enable the client credentials flow.
            options.AllowClientCredentialsFlow();
            //options.AllowAuthorizationCodeFlow();
            //options.AllowRefreshTokenFlow();
            //options.AllowPasswordFlow();


            options.AddEncryptionKey(new SymmetricSecurityKey(
            Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));


            // Register the signing and encryption credentials.
            options.AddDevelopmentEncryptionCertificate()
                   .AddDevelopmentSigningCertificate();

            // Register the ASP.NET Core host and configure the ASP.NET Core options.
            options.UseAspNetCore();
            options.AddEventHandler<HandleTokenRequestContext>(builder =>
                builder.UseScopedHandler<TokenRequest>());

        })
        .AddClient(options =>
        {
            options.AllowAuthorizationCodeFlow();

            options.UseSystemNetHttp();

            options.UseWebProviders()
                .AddDiscord(options =>
                {
                    options.SetClientId(DiscordOauth.clientid);
                    options.SetClientSecret(DiscordOauth.secret);
                    options.SetRedirectUri("auth/discord/callback");
                });
        })
        .AddValidation(options =>
         {
             options.UseLocalServer(); // Use local validation
             options.UseAspNetCore();
         });

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
        });

        builder.Services.AddAuthorization();
        builder.Services.AddHostedService<Worker>();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
#if DEBUG
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();
#endif

        var app = builder.Build();

        app.MapDefaultEndpoints();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

#if DEBUG
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "6MD Auth V1");
                options.RoutePrefix = string.Empty;
            });
#endif
        }



        {
            using var scope = app.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<DB.UserDB>();
            bool pendingMigrations = db.Database.GetPendingMigrations().Any();
            bool pendingModelChanges = db.Database.HasPendingModelChanges();
            if (pendingMigrations && !pendingModelChanges)
                await db.Database.MigrateAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
