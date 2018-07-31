using cqrsplayground.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.trade.service
{
    [Route("trades")]
    public class TradeService : ControllerBase, ITradeService, IDisposable
    {
        private ILogger<TradeService> _logger;
        private ITradeEventEmitter _tradeEventEmitter;
        private ITradeService _repository;
        private IDisposable _disposable;

        public TradeService(ILoggerFactory loggerFactory, ITradeService repository, ITradeEventEmitter tradeEventEmitter, ITradeEventProcessor tradeEventProcessor)
        {
            _logger = loggerFactory.CreateLogger<TradeService>();
            _tradeEventEmitter = tradeEventEmitter;
            _repository = repository;

            _disposable = tradeEventProcessor
              //refacto - filter out event
              .OnNewEvent
              .Subscribe(async change =>
              {
                  //refacto - Handle(trade) on ITradeEvent
                  if (change.GetType() != typeof(TradeCreated))
                  {
                      _logger.LogInformation($"Changing state of trade {change.TradeId}");

                      var trade = await _repository.Get(change.TradeId);

                      if (change.GetType() == typeof(TradeBooked))
                      {
                          trade.Status = TradeStatus.Booked;
                      }

                      if (change.GetType() == typeof(TradeValidated))
                      {
                          trade.Status = TradeStatus.Validated;
                      }

                      if (change.GetType() == typeof(TradeRejected))
                      {
                          trade.Status = TradeStatus.Rejected;
                      }

                  }

              });
        }

        [HttpPut]
        public async Task<TradeCreationResult> Create(TradeCreationDemand tradeCreationDemand)
        {
            var result = await _repository.Create(tradeCreationDemand);

            _logger.LogInformation($"Trade created [{result.TradeId}]");

            var @event = new TradeCreated()
            {
                TradeId = result.TradeId
            };

            await _tradeEventEmitter.Emit(@event);

            return result;
        }

        public void Dispose()
        {
            _disposable.Dispose();
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
    }
}
