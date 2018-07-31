using cqrsplayground.shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;

namespace cqrsplayground.compliance.service
{
    public class TradeComplianceService: IDisposable
    {
        private IDisposable _disposable;
        private ILogger _logger;
        private Random _rand;

        public TradeComplianceService(ILoggerFactory loggerFactory, ITradeEventEmitter tradeEventEmitter, ITradeEventProcessor tradeEventProcessor)
        {
            _logger = loggerFactory.CreateLogger<TradeComplianceService>();

            _rand = new Random();

            _disposable = tradeEventProcessor
              .OnNewEvent
              .Subscribe(change =>
              {
                  if (change.GetType() == typeof(TradeCreated))
                  {
                      _logger.LogInformation($"handle compliance for {change.TradeId}");

                      var isRejected = _rand.Next(0, 4) == 0;

                      if (isRejected) tradeEventEmitter.Emit(new TradeRejected()
                      {
                          TradeId = change.TradeId
                      });
                      else
                      {
                          tradeEventEmitter.Emit(new TradeValidated()
                          {
                              TradeId = change.TradeId
                          });
                      }
                  }

              });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
