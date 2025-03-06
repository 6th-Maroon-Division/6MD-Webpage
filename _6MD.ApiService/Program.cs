using Aspire.Npgsql.EntityFrameworkCore.PostgreSQL;
using Grpc.Core;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

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
