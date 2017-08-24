using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AuthServer.Model;
using IdentityModel.Client;
using Newtonsoft.Json;

namespace TestClient
{
    internal class Program
    {
        private static Uri AuthServer => new Uri(ConfigurationManager.AppSettings["AuthServer"]);
        private static Uri ResourceServer => new Uri(ConfigurationManager.AppSettings["ResourceServer"]);

        private static void Main(string[] args)
        {
            try
            {
                Run().Wait();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static async Task Run()
        {
            var token = await GetAccessTokenAsync();
            using (var client = new HttpClient {BaseAddress = AuthServer})
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var response = await client.GetAsync("api/v1/app-features");
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsAsync<AppFeature[]>();
                Console.WriteLine(JsonConvert.SerializeObject(content, Formatting.Indented));
            }
        }

        private static async Task<string> GetAccessTokenAsync()
        {
            var disco = await DiscoveryClient.GetAsync(AuthServer.OriginalString);

            using (var client =
                new TokenClient(disco.TokenEndpoint, "client-app", "0D7F43F5-5670-4C51-AD47-468A6B3C981D"))
            {
                var response = await client.RequestResourceOwnerPasswordAsync("user1", "p@ss", "auth");
                if (response.IsError)
                    throw new Exception($"Could not authenticate: {response.Error}");

                Console.WriteLine($"Access token: {response.AccessToken}");
                return response.AccessToken;
            }
        }
    }
}