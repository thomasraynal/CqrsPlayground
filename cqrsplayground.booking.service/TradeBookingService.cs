using cqrsplayground.shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.booking.service
{
    public class TradeBookingService : IDisposable
    {
        private IDisposable _disposable;
        private ILogger _logger;
 
        public TradeBookingService(ILoggerFactory loggerFactory, ITradeEventEmitter tradeEventEmitter, ITradeEventProcessor tradeEventProcessor)
        {
            _logger = loggerFactory.CreateLogger<TradeBookingService>();

            _disposable = tradeEventProcessor
              .OnNewEvent
              .Subscribe(change =>
              {
                  if (change.GetType() == typeof(TradeValidated))
                  {
                      _logger.LogInformation($"handle booking for {change.TradeId}");

                      tradeEventEmitter.Emit(new TradeBooked()
                      {
                          TradeId = change.TradeId
                      });
                  }

              });
        }

        public void Dispose()
        {
            _disposable.Dispose();
        }
    }
}
