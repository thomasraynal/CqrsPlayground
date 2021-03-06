﻿using cqrsplayground.shared;
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

        private async Task<ServiceToken> GetToken<TService>(string consumerKey, String endpoint, String realm)
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Realm", realm);
                var response = await client.GetAsync($"{endpoint}/{authenticationRoute}/{consumerKey}");
                if (response.StatusCode != HttpStatusCode.OK) throw new Exception(response.ToString());
                var token = JsonConvert.DeserializeObject<ServiceToken>(await response.Content.ReadAsStringAsync());
                return token;
            }
        }

        public TService GetClientFor<TService>(string consumerKey, String endpoint, String realm)
        {
            return RestService.For<TService>(new HttpClient(new AuthenticatedHttpClientHandler(async () => await GetToken<TService>(consumerKey, endpoint, realm))) { BaseAddress = new Uri(endpoint) });
        }
    }
}
