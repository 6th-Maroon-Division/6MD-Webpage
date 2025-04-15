using Microsoft.EntityFrameworkCore;
using OpenIddict.Abstractions;
using static OpenIddict.Abstractions.OpenIddictConstants;

namespace _6MD.AuthServer.services
{
    public class Worker : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;

        public Worker(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using var scope = _serviceProvider.CreateScope();

            var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();

            if (await manager.FindByClientIdAsync("service-worker") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "service-worker",
                    ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.GrantTypes.Password,
                        Permissions.GrantTypes.AuthorizationCode,
                        Permissions.GrantTypes.DeviceCode,
                        Permissions.GrantTypes.Implicit
                    }
                });
            }
            else if (await manager.FindByClientIdAsync("API") is null)
            {
                await manager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "API",
                    ClientSecret = "388D45FA-B36B-4988-BA59-B187D329C207",
                    Permissions =
                    {
                        Permissions.Endpoints.Token,
                        Permissions.GrantTypes.ClientCredentials,
                        Permissions.GrantTypes.RefreshToken,
                        Permissions.Endpoints.Introspection
                    }
                });
            }
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    }
}
