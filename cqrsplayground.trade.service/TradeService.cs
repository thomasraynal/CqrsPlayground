using cqrsplayground.authentication;
using cqrsplayground.authentication.service;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace cqrsplayground.trade.service
{
    [Route("")]
    public class TradeServiceAuth : AuthentificationService
    {
        public TradeServiceAuth(IAuthenticationService auth, IServiceInstance instance) : base(auth, instance)
        {
        }
    }

    [Route("trades")]
    public class TradeService : ControllerBase, ITradeService
    {
        private ITradeEventProcessor _tradeEventProcessor;
        private ITradeService _repository;

        public TradeService(
            ITradeService repository,  
            ITradeEventProcessor tradeEventProcessor)
        {
            _tradeEventProcessor = tradeEventProcessor;
            _repository = repository;
        }

        [HttpPut]
        [JwtBearerAuthorize]
        public async Task<TradeCreationResult> Create([FromBody] TradeCreationDto tradeCreationDemand)
        {

            var result = await _repository.Create(tradeCreationDemand);

            var @event = new TradeCreated()
            {
                TradeId = result.TradeId
            };

            await _tradeEventProcessor.Emit(@event);

            return result;

        }

        [HttpGet("{tradeId}")]
        [AllowAnonymous]
        public async Task<Trade> Get(Guid tradeId)
        {
            return await _repository.Get(tradeId); 
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Trade>> GetAll()
        {
            return await _repository.GetAll();
        }

        public Task Update([FromBody] Trade trade)
        {
            throw new NotImplementedException();
        }
    }
}
