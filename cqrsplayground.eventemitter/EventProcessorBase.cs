using cqrsplayground.shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
namespace cqrsplayground.eventemitter
{
    public abstract class EventProcessorBase : ITradeEventProcessor
    {
        protected ILogger _logger;
        private IConnectionFactory _connectionFactory;
        private EventingBasicConsumer _consumer;
        private string _queueName;
        private string _exchangeName;
        private string _consumerTag;
        private IModel _channel;
        private IConfiguration _configuration;

        private IDisposable _disposable;
        private ISubject<ITradeEvent> _tradeEventSubject;
        private IObservable<ITradeEvent> _onNewEvent;

        public abstract Task OnEvent(ITradeEvent @event);
        public abstract IEnumerable<Type> AllowedEvents { get; }

        public abstract String EventServiceName { get; }
        public abstract String EventExchangeName { get; }

        public EventProcessorBase(ILoggerFactory loggerFactory, IConfiguration configuration, IConnectionFactory connectionFactory, EventingBasicConsumer consumer)
        {
            _logger = loggerFactory.CreateLogger(GetType().Name);

            _tradeEventSubject = new Subject<ITradeEvent>();

            _connectionFactory = connectionFactory;

            _consumer = consumer;

            _configuration = configuration;

            _queueName = _configuration[$"queues:{EventServiceName}"];

            _exchangeName = _configuration[$"exchanges:{EventExchangeName}"];

            _channel = consumer.Model;

            _tradeEventSubject = new Subject<ITradeEvent>();

            _onNewEvent = _tradeEventSubject.AsObservable();

            _disposable = _onNewEvent.Subscribe(async change =>
            {
                _logger.LogInformation($"Changing state of trade {change.TradeId} with event {change.GetType().Name}");

                await OnEvent(change);

            });

            _channel.QueueDeclare(
               queue: _queueName,
               durable: false,
               exclusive: false,
               autoDelete: false,
               arguments: null
           );

            _channel.ExchangeDeclare(_exchangeName, "fanout");

            _channel.QueueBind(
                          queue: _queueName,
                          exchange: _exchangeName,
                          routingKey: "");

            _consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                var msg = Encoding.UTF8.GetString(body);
                var eventType = msg.GetEvenType();

                if (!AllowedEvents.Any(ev => ev == eventType)) return;

                var evt = (ITradeEvent)JsonConvert.DeserializeObject(msg, eventType);

                _logger.LogInformation($"Received incoming event {eventType}");

                _tradeEventSubject.OnNext(evt);

            };

        }

        public Task Emit(ITradeEvent @event)
        {
            using (var connection = _connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var payload = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(payload);

                    channel.BasicPublish(
                        exchange: _exchangeName,
                        routingKey: EventServiceName,
                        basicProperties: null,
                        body: body
                    );

                    _logger.LogInformation($"Emitted event {@event.GetType()}");

                }
            }

            return Task.CompletedTask;
        }

        public void Subscribe()
        {
            _consumerTag = _channel.BasicConsume(EventServiceName, true, _consumer);
        }

        public void Unsubscribe()
        {
            _channel.BasicCancel(_consumerTag);
        }

        public void Dispose()
        {
            Unsubscribe();

            _disposable.Dispose();
        }
    }
}
