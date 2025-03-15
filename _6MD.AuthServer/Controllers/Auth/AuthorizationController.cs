using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;
using static OpenIddict.Abstractions.OpenIddictConstants;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using OpenIddict.Validation.AspNetCore;
using System.Diagnostics.Eventing.Reader;

namespace _6MD.AuthServer.Controllers.Auth
{
    [Route("auth/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IOpenIddictApplicationManager _applicationManager;

        public AuthorizationController(IOpenIddictApplicationManager applicationManager)
            => _applicationManager = applicationManager;

        [HttpPost("token"), HttpPost("auth"), Produces("application/json")]
        public async Task<IActionResult> Exchange()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request is null)
            {
                throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
            }
            else if (request.IsClientCredentialsGrantType())
            {
                await HttpContext.AuthenticateAsync(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
                // Note: the client credentials are automatically validated by OpenIddict:
                // if client_id or client_secret are invalid, this action won't be invoked.

                var application = await _applicationManager.FindByClientIdAsync(request.ClientId ?? "") ??
                    throw new InvalidOperationException("The application cannot be found.");
                // Create a new ClaimsIdentity containing the claims that
                // will be used to create an id_token, a token or a code.
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme, Claims.Name, Claims.Role);
                identity.AddClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application) ?? "");
                identity.AddClaim(Claims.ClientId, await _applicationManager.GetClientIdAsync(application) ?? "");
                if (request.ClientId == "API")
                    identity.AddClaim(Claims.Audience, "API");
                identity.SetScopes(request.GetScopes().Add(Permissions.Prefixes.Scope + "test"));

                // Use the client_id as the subject identifier.
                identity.SetClaim(Claims.Subject, await _applicationManager.GetClientIdAsync(application));
                identity.SetClaim(Claims.Name, await _applicationManager.GetDisplayNameAsync(application));

                identity.SetDestinations(static claim => claim.Type switch
                {
                    // Allow the "name" claim to be stored in both the access and identity tokens
                    // when the "profile" scope was granted (by calling principal.SetScopes(...)).
                    Claims.Name when claim?.Subject?.HasScope(Scopes.Profile) ?? false
                        => [Destinations.AccessToken, Destinations.IdentityToken],

                    // Otherwise, only store the claim in the access tokens.
                    _ => [Destinations.AccessToken]
                });
                var ticket = new AuthenticationTicket(new ClaimsPrincipal(identity), new AuthenticationProperties(), OpenIddict.Server.AspNetCore.OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);

                return SignIn(ticket.Principal, ticket.Properties, OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
            }

            throw new NotImplementedException("The specified grant is not implemented.");
        }

        [HttpPost("Introspection"), Produces("application/json")]
        public async Task<IActionResult> Introspect()
        {
            var request = HttpContext.GetOpenIddictServerRequest();
            if (request is null)
            {
                throw new InvalidOperationException("Invalid OpenID Connect request.");
            }

            return Ok(new { active = true });
        }
    }
}
