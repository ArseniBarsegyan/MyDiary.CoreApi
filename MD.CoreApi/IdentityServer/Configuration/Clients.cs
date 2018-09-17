﻿using System.Collections.Generic;
using IdentityServer4;
using IdentityServer4.Models;

namespace IdentityServer.Configuration
{
    public class Clients
    {
        public static IEnumerable<Client> Get()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId = "MDAngular",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets =
                    {
                        new Secret("MDSecretKey".Sha256())
                    },
                    AllowedScopes = { "MD.CoreApi" }
                },

                // Angular client
                new Client {
                    ClientId = "angular_spa",
                    ClientName = "Angular 4 Client",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    AllowedScopes = new List<string> { "openid", "profile", "MD.CoreApi" },
                    RedirectUris = new List<string> { "http://localhost:4200/auth-callback" },
                    PostLogoutRedirectUris = new List<string> { "http://localhost:4200/" },
                    AllowedCorsOrigins = new List<string> { "http://localhost:4200" },
                    AllowAccessTokensViaBrowser = true
                },

                // OpenID Connect hybrid flow and client credentials client (MVC)
                new Client
                {
                    ClientId = "MDMVC",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,

                    RequireConsent = true,

                    ClientSecrets =
                    {
                        new Secret("MDSecretKey".Sha256())
                    },

                    RedirectUris = { "http://localhost:51866/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:51866/signout-callback-oidc" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "MD.CoreApi"
                    },
                    AllowOfflineAccess = true
                }
            };
        }
    }
}