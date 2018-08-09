//using Microsoft.AspNetCore.Http.Extensions;
//using Microsoft.AspNetCore.Mvc;
//using Newtonsoft.Json;
//using Steeltoe.Common.Discovery;
//using System;
//using System.Collections.Generic;
//using System.Net.Http;
//using System.Text;
//using System.Threading.Tasks;

//namespace cqrsplayground.gateway
//{
//    //refacto : should have a proxy middleware
//    public abstract class ServiceGateway: ControllerBase
//    {
//        private IDiscoveryClient _client;
//        private DiscoveryHttpClientHandler _handler;
//        public abstract String Realm { get; }

//        private Uri MapUri(String url)
//        {
//            var uri = new Uri(url);
//            var forwardedUrl = uri.AbsoluteUri.Replace(uri.Authority, Realm);
//            return new Uri(forwardedUrl);
//        }

//        private HttpClient CreateClient()
//        {
//            return new HttpClient(_handler, false);
//        }

//        public ServiceGateway(IDiscoveryClient client)
//        {
//            _client = client;
//            _handler = new DiscoveryHttpClientHandler(client);
//        }

//        //refacto : forwarding should forward target response on the middleware layer
//        public async Task<T> Forward<T>()
//        {
//            var url = UriHelper.GetDisplayUrl(Request);

//            var forwadedRequest = Request.Forward(MapUri(url));

//            using (var client = CreateClient())
//            {
//                var response = await client.SendAsync(forwadedRequest);
//                var result = JsonConvert.DeserializeObject<T>(await response.Content.ReadAsStringAsync());
//                return result;

//            }

//        }
//    }
//}
