using System.Collections.Immutable;
using Duende.IdentityServer.Models;

namespace IdentityServer.Data.SeedData;

public static class Config
{
    public static readonly IImmutableList<IdentityResource> IdentityResources = ImmutableArray.Create<IdentityResource>(
        new IdentityResources.OpenId(),
        new IdentityResources.Profile(),
        new IdentityResources.Email()
    );

    public static readonly IImmutableList<ApiScope> ApiScopes = ImmutableArray.Create(
        new ApiScope("graphql-gateway", "GraphQL Gateway"),
        new ApiScope("communication", "Communication API"),
        new ApiScope("personal-data", "Personal Data API"),
        new ApiScope("master-data", "Master Data API")
    );

    public static readonly IImmutableList<ApiResource> ApiResources = ImmutableArray.Create(
        new ApiResource("graphql-gateway", "GraphQL Gateway")
        {
            Scopes = { "graphql-gateway" },
            UserClaims = { "email", "role", "permission" }
        },
        new ApiResource("communication", "Communication API")
        {
            Scopes = { "communication" }
        },
        new ApiResource("personal-data", "Personal Data API")
        {
            Scopes = { "personal-data" }
        },
        new ApiResource("master-data", "Master Data API")
        {
            Scopes = { "master-data" }
        }
    );

    public static readonly IImmutableList<Client> Clients = ImmutableArray.Create(
        new Client
        {
            ClientId = "Promag-SPA",
            ClientName = "Promag SPA",
            ClientSecrets =
            {
                new Secret("nSkZKHlfW/cgXeRiWBR9246YFn0wbj2K6E+6xVYQ5mo=")
            },

            AllowedGrantTypes = GrantTypes.Implicit,

            AllowedCorsOrigins =
            {
                "https://localhost:5102",
                "http://localhost:5102",
                "https://promag.minhtrandev.com",
                "http://promag.minhtrandev.com"
            },

            RedirectUris =
            {
                "https://localhost:5102/auth/callback",
                "http://localhost:5102/auth/callback",
                "https://promag.minhtrandev.com/auth/callback",
                "http://promag.minhtrandev.com/auth/callback"
            },

            AllowedScopes =
            {
                "openid",
                "email",
                "profile",
                "master-data",
                "personal-data",
                "communication",
                "graphql-gateway"
            },

            AlwaysIncludeUserClaimsInIdToken = true,
            AllowAccessTokensViaBrowser = true,
            RequirePkce = false,
            IncludeJwtId = false,
            ClientClaimsPrefix = "client_"
        },
        new Client
        {
            ClientId = "Postman-Client",
            ClientName = "Postman Client",

            AllowedGrantTypes = GrantTypes.Implicit,

            AllowedCorsOrigins =
            {
                "https://app.getpostman.com",
                "https://www.getpostman.com"
            },

            PostLogoutRedirectUris =
            {
                "https://app.getpostman.com",
                "https://www.getpostman.com"
            },

            RedirectUris =
            {
                "https://app.getpostman.com/oauth2/callback",
                "https://www.getpostman.com/oauth2/callbackk"
            },

            AllowedScopes =
            {
                "openid",
                "email",
                "profile",
                "master-data",
                "personal-data",
                "communication",
                "graphql-gateway"
            },

            AlwaysIncludeUserClaimsInIdToken = true,
            AllowAccessTokensViaBrowser = true,
            RequirePkce = false,
            IncludeJwtId = false,
            ClientClaimsPrefix = "client_"
        }
    );
}