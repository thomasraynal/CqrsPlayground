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

namespace cqrsplayground.compliance
{
    public class TradeComplianceServiceEventProcessor : EventProcessorBase, IDisposable
    {
        private Random _rand;
        private IEnumerable<Type> _allowedEvents;
       
        public TradeComplianceServiceEventProcessor(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory, IConfiguration configuration, EventingBasicConsumer consumer) : base(loggerFactory, configuration, connectionFactory, consumer)
        {
            _rand = new Random();

            _allowedEvents = new List<Type>()
            {
                typeof(TradeAcknowleged)
            };
        }
       
        public override IEnumerable<Type> AllowedEvents => _allowedEvents;

        public override string EventServiceName => ServiceConstants.ComplianceEventService;

        public override string EventExchangeName => ServiceConstants.EventExchange;

        public async override Task OnEvent(ITradeEvent @event)
        {
            //process trade...
            await Task.Delay(1000);

            var isRejected = _rand.Next(0, 4) == 0;

            if (isRejected)
            {
                await Emit(new TradeRejected()
                {
                    TradeId = @event.TradeId
                });

                _logger.LogInformation($"Trade {@event.TradeId} has been rejected");


            }
            else
            {
                await Emit(new TradeValidated()
                {
                    TradeId = @event.TradeId
                });

                _logger.LogInformation($"Trade {@event.TradeId} has been validated");
            }
        }
    }
}
