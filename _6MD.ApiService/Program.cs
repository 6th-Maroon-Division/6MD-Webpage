using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Validation.AspNetCore;
using System.Security.Cryptography.X509Certificates;

var builder = WebApplication.CreateBuilder(args);

string authserver = Environment.GetEnvironmentVariable("services__Auth-Server__https__0") ?? throw new Exception("AuthServer not found!");

// Add service defaults & Aspire client integrations.
builder.AddServiceDefaults();

// Add services to the container.
builder.Services.AddProblemDetails();
builder.Services.AddControllers();
//builder.AddNpgsqlDbContext<DataStructure.DB>("MainDB");
builder.Services.AddDbContext<DataStructure.DB>(options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("MainDB"));
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
#if DEBUG
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
#endif

builder.Services.AddOpenIddict()
    .AddValidation(options =>
    {
        options.SetIssuer(authserver);
        //options.SetIssuer("https://Auth-Server");
        //options.AddAudiences("API");
        //options.AddEncryptionKey(new SymmetricSecurityKey(
        //    Convert.FromBase64String("DRjd/GnduI3Efzen9V9BvbNUfc/VKgXltV7Kbk9sMkY=")));

        options.UseIntrospection()
            .SetClientId("API")
            .SetClientSecret("388D45FA-B36B-4988-BA59-B187D329C207")
            .UseSystemNetHttp();


        options.UseAspNetCore();
    });



builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
    options.DefaultAuthenticateScheme = OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme;
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
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
    //if (db.Database.GetPendingMigrations().Any())
    //    await db.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapDefaultEndpoints();
app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();
