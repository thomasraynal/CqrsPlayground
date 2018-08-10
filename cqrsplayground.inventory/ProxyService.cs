using Steeltoe.Common.Discovery;
using System.Net.Http;

namespace cqrsplayground.gateway
{
    public class ProxyService
    {
        public ProxyService(IDiscoveryClient client)
        {
            _handler = new DiscoveryHttpClientHandler(client);

            Client = new HttpClient(_handler, false);
        }

        private DiscoveryHttpClientHandler _handler;

        public HttpClient Client { get; private set; }
    }
}
