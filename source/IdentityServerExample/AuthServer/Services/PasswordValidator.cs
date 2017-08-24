using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json.Linq;

namespace AuthServer.Services
{
    public class PasswordValidator : IResourceOwnerPasswordValidator
    {
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            if (context.UserName == "user1" && context.Password == "p@ss")
            {
                context.Result = new GrantValidationResult("1", OidcConstants.AuthenticationMethods.Password);
                return;
            }
            if (context.UserName == "SUBSYSTEMAUTH")
            {
                using (var client = new HttpClient()
                {
                    // Todo: make URI configurable
                    BaseAddress = new Uri("http://localhost:60692/"),
                    DefaultRequestHeaders =
                    {
                        Authorization = new AuthenticationHeaderValue("Bearer", context.Password)
                    }
                })
                {
                    var response = await client.PostAsync("api/v1/token-validation", null);
                    if (response.IsSuccessStatusCode)
                    {
                        var info = JObject.Parse(await response.Content.ReadAsStringAsync());
                        context.Result = new GrantValidationResult(info.Value<string>("sub"), OidcConstants.AuthenticationMethods.Password);
                    }
                }
            }

        }
    }
}