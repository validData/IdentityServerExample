using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthServer.Model;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ResourceServer.Controllers
{
    public class SecuredController : Controller
    {
        private readonly AuthServerConfig _configuration;

        public SecuredController(IOptions<AuthServerConfig> configuration)
        {
            _configuration = (configuration ?? throw new ArgumentNullException(nameof(configuration))).Value;
        }

        protected async Task RequirePermissionAsync(string requiredPermission)
        {
            var userId = User.FindFirst(JwtClaimTypes.Subject)?.Value;
            var disco = await DiscoveryClient.GetAsync(new Uri(_configuration.AuthServerUrl).OriginalString);

            using (var client = new TokenClient(
                disco.TokenEndpoint,
                _configuration.ClientId,
                _configuration.ClientSecret))
            {
                var response = await client.RequestClientCredentialsAsync("auth");
                if (response.IsError)
                    throw new Exception("Could not authenticate with authentication server");

                using (var httpClient = CreateHttpClient(response.AccessToken))
                {
                    var permissionsResponse = await httpClient.GetAsync(GetPermissionUrl(userId));
                    permissionsResponse.EnsureSuccessStatusCode();

                    var permissions = await permissionsResponse.Content.ReadAsAsync<PermissionResponse>();
                    if (permissions.Permissions.All(c => c.Name != requiredPermission))
                        throw new Exception("Permission denied");
                }
            }
        }

        private static string GetPermissionUrl(string userId)
        {
            return $"api/v1/permissions/?userId={WebUtility.UrlEncode(userId)}";
        }

        private HttpClient CreateHttpClient(string accessToken)
        {
            return new HttpClient
            {
                BaseAddress = new Uri(_configuration.AuthServerUrl),
                DefaultRequestHeaders =
                {
                    Authorization = new AuthenticationHeaderValue("Bearer", accessToken)
                }
            };
        }
    }
}