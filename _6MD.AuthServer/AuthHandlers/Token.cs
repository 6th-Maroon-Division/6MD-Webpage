using System.Security.Claims;
using System.Threading.Tasks;
using _6MD.AuthServer.DB;
using _6MD.AuthServer.utils.Exceptions;
using Microsoft.Extensions.Logging;
using OpenIddict.Abstractions;
using OpenIddict.Server;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using static OpenIddict.Server.OpenIddictServerEvents;


namespace _6MD.AuthServer.AuthHandlers
{

    public class Token : IOpenIddictServerHandler<HandleTokenRequestContext>
    {
        private readonly IOpenIddictApplicationManager _applicationManager;
        private readonly ILogger<Token> _logger;
        private readonly UserDB _context;

        public Token(
            IOpenIddictApplicationManager applicationManager,
            ILogger<Token> logger,
            UserDB context)
        {
            _applicationManager = applicationManager;
            _logger = logger;
            _context = context;
        }

        public async ValueTask HandleAsync(HandleTokenRequestContext context)
        {
            _logger.LogDebug("🚀 TokenRequest Handler started!");

            if (context.Request is null)
            {
                _logger.LogError("❌ No OpenIddict request found!");
                return;
            }

            _logger.LogDebug("Handling token request for grant type: {GrantType}", context.Request.GrantType);

            if (context.Request.IsClientCredentialsGrantType())
            {
                _logger.LogDebug("Processing client credentials grant type for ClientId: {ClientId}", context.ClientId);

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

                _logger.LogDebug("✅ Application found for ClientId: {ClientId}", context.ClientId);

                // Create claims identity
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
                identity.AddClaim(OpenIddictConstants.Claims.Subject, context.ClientId);
                identity.AddClaim(OpenIddictConstants.Claims.ClientId, context.ClientId);

                if (context.ClientId == "API")
                {
                    identity.AddClaim(OpenIddictConstants.Claims.Audience, "API");
                }

                identity.SetScopes(context.Request.GetScopes());
                identity.AddClaim(OpenIddictConstants.Claims.Name, "Test Client");

                _logger.LogDebug("✅ Claims identity created");

                var principal = new ClaimsPrincipal(identity);
                context.Principal = principal;

                // ✅ Ensure OpenIddict processes the request
                context.SignIn(principal);

                _logger.LogDebug("✅ Principal assigned, request handled.");
            }
            else if (context.Request.IsPasswordGrantType())
            {
                User user;
                try
                {
                    user = new User(_context, context.Request.Username ?? "", context.Request.Password ?? "");
                }
                catch (UserNotFoundException)
                {
                    _logger.LogWarning("❌ User not found: {Username}", context.Request.Username);
                    context.Reject(
                        error: OpenIddictConstants.Errors.InvalidGrant,
                        description: "The username or password is invalid."
                    );
                    return;
                }
                catch (PasswordNotSetException)
                {
                    _logger.LogWarning("❌ Password not set for user: {Username}", context.Request.Username);
                    context.Reject(
                        error: OpenIddictConstants.Errors.InvalidGrant,
                        description: "The username or password is invalid."
                    );
                    return;
                }

                ClaimsIdentity identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
                identity.AddClaim(OpenIddictConstants.Claims.Subject, user.GUID);
                identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email);
                identity.AddClaim(OpenIddictConstants.Claims.Username, context.Request.Username ?? "");
                identity.AddClaim(OpenIddictConstants.Claims.Name, context.Request.Username ?? "");
                Dictionary<string, int> dictionary = user.getPermissions();
                dictionary.Add("tester", 200);
                foreach (var (action, level) in dictionary)
                {
                    Claim claim = new("perm", $"{action}:{level}",
                        OpenIddictConstants.Destinations.AccessToken);
                    claim.SetDestinations(OpenIddictConstants.Destinations.AccessToken);
                    identity.AddClaim(claim);
                }
                identity.SetScopes(context.Request.GetScopes());

                var principal = new ClaimsPrincipal(identity);
                context.SignIn(principal);
            }
            else
            {
                _logger.LogWarning("❌ Unsupported grant type: {GrantType}", context.Request.GrantType);
                context.Reject(
                    error: OpenIddictConstants.Errors.UnsupportedGrantType,
                    description: "The specified grant type is not implemented."
                );
            }

            _logger.LogDebug("🚀 TokenRequest execution completed!");
        }
    }
}

