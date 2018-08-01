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

namespace cqrsplayground.trade.service
{
    public class TradeServiceEventProcessor : EventProcessorBase, IDisposable
    {
        private ITradeService _repository;

        private IEnumerable<Type> _allowedEvents;

        public override string EventServiceName => ServiceConstants.TradeEventService;

        public override string EventExchangeName => ServiceConstants.EventExchange;

        public TradeServiceEventProcessor(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory, IConfiguration configuration, ITradeService service, EventingBasicConsumer consumer) : base(loggerFactory, configuration, connectionFactory, consumer)
        {
            _repository = service;

            _allowedEvents = new List<Type>()
            {
                typeof(TradeCreated),
                typeof(TradeAcknowleged),
                typeof(TradeValidated),
                typeof(TradeRejected),
                typeof(TradeBooked)
            };
        }

        public override IEnumerable<Type> AllowedEvents => _allowedEvents;

        public async override Task OnEvent(ITradeEvent @event)
        {
            var trade = await _repository.Get(@event.TradeId);

            @event.Handle(trade);

            await _repository.Update(trade);

            if (@event is TradeCreated)
            {
                _logger.LogInformation($"Trade {@event.TradeId} has been acknowledged");

                await Emit(new TradeAcknowleged()
                {
                    TradeId = @event.TradeId
                });

            }
        }
    }
}
