using cqrsplayground.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.trade.service
{
    [Route("trades")]
    public class TradeService : ControllerBase, ITradeService
    {
        private ILogger<TradeService> _logger;
        private ITradeEventProcessor _tradeEventProcessor;
        private ITradeService _repository;
    
        public TradeService(ILoggerFactory loggerFactory, ITradeService repository, ITradeEventProcessor tradeEventProcessor)
        {
            _logger = loggerFactory.CreateLogger<TradeService>();
            _tradeEventProcessor = tradeEventProcessor;
            _repository = repository;
        }

        [HttpPut]
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

        [HttpGet("tradeId")]
        public async Task<Trade> Get(Guid tradeId)
        {
            return await _repository.Get(tradeId); 
        }

        [HttpGet]
        public async Task<IEnumerable<Trade>> GetAll()
        {
            return await _repository.GetAll();
        }

        public Task Update(Trade trade)
        {
            throw new NotImplementedException();
        }
    }
}
