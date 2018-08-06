using cqrsplayground.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.trade.service
{
    [Route("trades")]
    public class TradeRepository : ControllerBase, ITradeService
    {
        private ILogger<TradeRepository> _logger;
        private ITradeService _repository;
    
        public TradeRepository(ILoggerFactory loggerFactory, ITradeService repository)
        {
            _logger = loggerFactory.CreateLogger<TradeRepository>();
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

            return result;
        }

        [HttpGet("{tradeId}")]
        public async Task<Trade> Get(Guid tradeId)
        {
            return await _repository.Get(tradeId); 
        }

        [HttpGet]
        public async Task<IEnumerable<Trade>> GetAll()
        {
            return await _repository.GetAll();
        }

        [HttpPatch]
        public async Task Update([FromBody]  Trade trade)
        {
            await _repository.Update(trade);
        }
    }
}
