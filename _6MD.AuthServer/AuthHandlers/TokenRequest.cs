using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;

public class TokenRequest : IOpenIddictServerHandler<HandleTokenRequestContext>
{
    private readonly IOpenIddictApplicationManager _applicationManager;
    private readonly ILogger<TokenRequest> _logger;

    public TokenRequest(
        IOpenIddictApplicationManager applicationManager,
        ILogger<TokenRequest> logger)
    {
        _applicationManager = applicationManager;
        _logger = logger;
    }

    public async ValueTask HandleAsync(HandleTokenRequestContext context)
    {
        _logger.LogInformation("🚀 TokenRequest Handler started!");

        if (context.Request is null)
        {
            _logger.LogError("❌ No OpenIddict request found!");
            return;
        }

        _logger.LogInformation("Handling token request for grant type: {GrantType}", context.Request.GrantType);

        if (context.Request.IsClientCredentialsGrantType())
        {
            _logger.LogInformation("Processing client credentials grant type for ClientId: {ClientId}", context.ClientId);

            var application = await _applicationManager.FindByClientIdAsync(context.ClientId ?? "");
            if (application is null)
            {
                _logger.LogError("❌ Application not found for ClientId: {ClientId}", context.ClientId);
                context.Reject(
                    error: OpenIddictConstants.Errors.InvalidClient,
                    description: "The application cannot be found."
                );
                return;
            }

            _logger.LogInformation("✅ Application found for ClientId: {ClientId}", context.ClientId);

            // Create claims identity
            var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
            identity.AddClaim(OpenIddictConstants.Claims.Subject, context.ClientId);
            identity.AddClaim(OpenIddictConstants.Claims.ClientId, context.ClientId);

            if (context.ClientId == "API")
            {
                identity.AddClaim(OpenIddictConstants.Claims.Audience, "API");
            }

            identity.SetScopes(context.Request.GetScopes());
            identity.SetClaim(OpenIddictConstants.Claims.Name, "Test Client");

            _logger.LogInformation("✅ Claims identity created");

            var principal = new ClaimsPrincipal(identity);
            context.Principal = principal;

            // ✅ Ensure OpenIddict processes the request
            context.SignIn(principal);

            _logger.LogInformation("✅ Principal assigned, request handled.");
        }
        else
        {
            _logger.LogWarning("❌ Unsupported grant type: {GrantType}", context.Request.GrantType);
            context.Reject(
                error: OpenIddictConstants.Errors.UnsupportedGrantType,
                description: "The specified grant type is not implemented."
            );
        }

        _logger.LogInformation("🚀 TokenRequest execution completed!");
    }
}
