using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.booking.service
{
    public class TradeBookingServiceEventProcessor : EventProcessorBase, IDisposable
    {
        private IEnumerable<String> _tradeBooks;
        private IEnumerable<Type> _allowedEvents;

        public TradeBookingServiceEventProcessor(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory, IConfiguration configuration, EventingBasicConsumer consumer) : base(loggerFactory, configuration, connectionFactory, consumer)
        {
            _allowedEvents = new List<Type>()
            {
                typeof(TradeValidated)
            };

            _tradeBooks = new List<String>()
            {
                "DESK EQU",
                "DESK DIV",
                "DESK FIX"
            };
        }

        public override IEnumerable<Type> AllowedEvents => _allowedEvents;

        public override string EventServiceName => ServiceConstants.BookingEventService;

        public override string EventExchangeName => ServiceConstants.EventExchange;

        public async override Task OnEvent(ITradeEvent @event)
        {
            _logger.LogInformation($"Trade {@event.TradeId} has been booked");

            await Emit(new TradeBooked()
            {
                TradeId = @event.TradeId,
                TradeBook = _tradeBooks.Random()
            });

        }
    }
}
