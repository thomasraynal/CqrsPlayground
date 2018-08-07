using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.Transport;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;

namespace cqrsplayground.gateway
{
    public class ServiceEurekaHttpClient : EurekaHttpClient
    {
        private IOptionsMonitor<EurekaClientOptions> _configOptions;

        protected override IEurekaClientConfig Config
        {
            get
            {
                return _configOptions.CurrentValue;
            }
        }

        public ServiceEurekaHttpClient(IOptionsMonitor<EurekaClientOptions> config, ILoggerFactory logFactory = null)
        {
            _config = null;
            _configOptions = config ?? throw new ArgumentNullException(nameof(config));

            var handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };

            _client = new HttpClient(handler);

            Initialize(new Dictionary<string, string>(), logFactory);
        }
    }
}
