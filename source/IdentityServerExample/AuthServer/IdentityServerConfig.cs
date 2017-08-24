using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace AuthServer
{
    public class IdentityServerConfig
    {
        public static IEnumerable<ApiResource> GetApiResources()
        {
            yield return new ApiResource("auth");
            yield return new ApiResource("res1");
        }

        public static IEnumerable<Client> GetClients()
        {
            yield return new Client
            {
                ClientId = "client-app",
                ClientName = "Client application",
                AccessTokenType = AccessTokenType.Jwt,
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                RequireClientSecret = false,
                ClientSecrets = new[] {new Secret("0D7F43F5-5670-4C51-AD47-468A6B3C981D".Sha256())},
                AllowedScopes = new[] {"auth", "res1"}
            };

            yield return new Client
            {
                ClientId = "resource-server",
                ClientName = "Resource to auth server communication",
                AccessTokenType = AccessTokenType.Jwt,
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets = new[] {new Secret("0A3A2A81-B7BA-4C52-B9AA-1C04B666DD57".Sha256())},
                AllowedScopes = new[] {"auth"},
                // may request permissions
                Claims = new[] {new Claim("permcl", "true", ClaimValueTypes.Boolean)}
            };
        }


        public static IEnumerable<TestUser> GetUsers()
        {
            yield return new TestUser
            {
                SubjectId = "1",
                Username = "user1",
                Password = "p@ss"
            };
        }
    }
}