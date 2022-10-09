// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using System.Collections.Generic;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace IdentityServerAspNetIdentity
{
    public static class Config
    {
        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };


        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>
            {
                new ApiScope("staff-training-api", "API")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>
            {               
                // interactive ASP.NET Core MVC client
                new Client
                {
                    ClientId = "staff-training-web",
                    ClientSecrets = { new Secret("secret") },
                    RequireClientSecret = false,
                    AllowedGrantTypes = GrantTypes.Code,
                    AccessTokenLifetime = 60,
                    RefreshTokenExpiration = TokenExpiration.Absolute,
                    AbsoluteRefreshTokenLifetime = 3600,
                    AllowOfflineAccess = true,
                    // where to redirect to after login
                    RedirectUris = { "http://localhost:4200/index.html", 
                        "http://localhost:4200/assets/silent-refresh.html",
                        "https://localhost:44325/swagger/oauth2-redirect.html", 
                        "https://stafftraining-api.azurewebsites.net/swagger/oauth2-redirect.html"
                    },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:4200/signout-callback-oidc" },
                    AllowedCorsOrigins = new[] { "http://localhost:4200", "https://localhost:44325", "https://stafftraining-api.azurewebsites.net"},
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "staff-training-api",
                        "staff-training-profile"
                    },
                }
            };
    }
}