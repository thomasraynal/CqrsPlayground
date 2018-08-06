using cqrsplayground.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.inventory
{
    [Route("trades")]
    public class TradeService : ControllerBase, ITradeService
    {
        private ILogger<TradeService> _logger;
        private DiscoveryHttpClientHandler _handler;
        private const string realm = "trades";

        public TradeService(ILoggerFactory loggerFactory, IDiscoveryClient client)
        {
            _logger = loggerFactory.CreateLogger<TradeService>();
            _handler = new DiscoveryHttpClientHandler(client);
        }

        private String BuildUri(String query = null)
        {
            var baseUri = $"http://{realm}";

            if (null == query) return baseUri;

            return $"{baseUri}//{query}";
        }

        private HttpClient CreateClient()
        {
            return new HttpClient(_handler, false);
        }

        [HttpPut]
        public Task<TradeCreationResult> Create([FromBody] TradeCreationDto tradeCreationDemand)
        {
            throw new NotImplementedException();
        }

        [HttpGet("{tradeId}")]
        public Task<Trade> Get(Guid tradeId)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        public async Task<IEnumerable<Trade>> GetAll()
        {
            using (var client = CreateClient())
            {
                var uri = BuildUri();
                var json = await client.GetStringAsync(uri);
                var result = JsonConvert.DeserializeObject<List<Trade>>(json);
                return result;
            }
        }

        public Task Update(Trade trade)
        {
            throw new NotImplementedException();
        }
    }
}
