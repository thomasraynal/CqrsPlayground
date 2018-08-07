using cqrsplayground.authentication;
using cqrsplayground.authentication.service;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Eureka;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.gateway
{
    //refacto : should have a proxy middleware
    [Route("")]
    public class TradeServiceAuth : ServiceGateway, IAuthenticatedService
    {
        public override string Realm => "trade";

        public TradeServiceAuth(IDiscoveryClient client) : base(client)
        {
        }

        [HttpGet("auth/{consumerServiceKey}")]
        public async Task<ServiceToken> GetServiceToken(String consumerServiceKey)
        {
            return await Forward<ServiceToken>();
        }

    }

    //refacto : should have a proxy middleware
    [Route("trades")]
    public class TradeServiceGateway : ServiceGateway, ITradeService
    {
        public override string Realm => "trade";

        public TradeServiceGateway(IDiscoveryClient client) : base(client)
        {
        }

        [HttpPut]
        public async Task<TradeCreationResult> Create([FromBody] TradeCreationDto tradeCreationDemand)
        {
            return await Forward<TradeCreationResult>();
        }

        [HttpGet("{tradeId}")]
        public async Task<Trade> Get(Guid tradeId)
        {
            return await Forward<Trade>();
        }

        [HttpGet]
        public async Task<IEnumerable<Trade>> GetAll()
        {
            var result = await Forward<List<Trade>>();
            return result.AsEnumerable();
        }

        public Task Update(Trade trade)
        {
            throw new NotImplementedException();
        }
    }
}
