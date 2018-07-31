using cqrsplayground.shared;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.eventemitter
{
    public class RabbitMQEventProcessor : ITradeEventProcessor
    {
        private ILogger<RabbitMQEventEmitter> _logger;
        private IConnectionFactory _connectionFactory;
        private EventingBasicConsumer _consumer;
        private string _consumerTag;
        private IModel _channel;

        private ISubject<ITradeEvent> _tradeEventSubject;
        private IObservable<ITradeEvent> _onNewEvent;

        public IObservable<ITradeEvent> OnNewEvent
        {
            get
            {
                return _onNewEvent;
            }
        }

        public RabbitMQEventProcessor(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory, EventingBasicConsumer consumer)
        {
            _logger = loggerFactory.CreateLogger<RabbitMQEventEmitter>();

            _tradeEventSubject = new Subject<ITradeEvent>();

            _connectionFactory = connectionFactory;

            _consumer = consumer;

            _channel = consumer.Model;

            _tradeEventSubject = new Subject<ITradeEvent>();

            _onNewEvent = _tradeEventSubject.AsObservable();

            Initialize();
        }

        private void Initialize()
        {
            _channel.QueueDeclare(
                queue: ServiceConstants.RabbitMQQueue,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            _consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);
                var eventType = msg.GetEvenType();

                var evt = (ITradeEvent) JsonConvert.DeserializeObject(msg, eventType);
                _logger.LogInformation($"Received incoming event {eventType}");

                _tradeEventSubject.OnNext(evt);

                _channel.BasicAck(ea.DeliveryTag, false);
            };
        }

        public void Subscribe()
        {
            _consumerTag = _channel.BasicConsume(ServiceConstants.RabbitMQQueue, false, _consumer);
            _logger.LogInformation("Subscribed to queue.");
        }

        public void Unsubscribe()
        {
            _channel.BasicCancel(_consumerTag);
            _logger.LogInformation("Unsubscribed from queue.");
        }
    }
}
