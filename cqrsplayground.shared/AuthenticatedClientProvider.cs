using cqrsplayground.shared;
using Newtonsoft.Json;
using Refit;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public class AuthenticatedClientProvider : IAuthenticatedClientProvider
    {
        public const string authenticationRoute = "auth";

        private async Task<ServiceToken> GetToken<TService>(string consumerKey, String endpoint)
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync($"{endpoint}/{authenticationRoute}/{consumerKey}");
                if (response.StatusCode != HttpStatusCode.OK) throw new UnauthorizedAccessException(typeof(TService).ToString());
                var token = JsonConvert.DeserializeObject<ServiceToken>(await response.Content.ReadAsStringAsync());
                return token;
            }
        }

        public TService GetClientFor<TService>(string consumerKey, String endpoint)
        {
            return RestService.For<TService>(new HttpClient(new AuthenticatedHttpClientHandler(async () => await GetToken<TService>(consumerKey, endpoint))) { BaseAddress = new Uri(endpoint) });
        }
    }
}
