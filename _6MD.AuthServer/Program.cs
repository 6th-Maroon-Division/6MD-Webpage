
using Microsoft.EntityFrameworkCore;

namespace _6MD.AuthServer;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.AddServiceDefaults();

        // Add services to the container.

        builder.Services.AddDbContext<DB.UserDB>(options =>
            options.UseNpgsql("UserDB").UseOpenIddict()
            );

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
            if (db.Database.GetPendingMigrations().Any())
                await db.Database.MigrateAsync();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}
