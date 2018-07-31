using cqrsplayground.shared;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.eventemitter
{
    public class RabbitMQEventEmitter : ITradeEventEmitter
    {
        private ILogger<RabbitMQEventEmitter> _logger;
        private IConnectionFactory _connectionFactory;

        public RabbitMQEventEmitter(ILoggerFactory loggerFactory, IConnectionFactory connectionFactory)
        {
            _logger = loggerFactory.CreateLogger<RabbitMQEventEmitter>();
            _connectionFactory = connectionFactory; 
        }

        public Task Emit(ITradeEvent @event) 
        {
            using (var conn = _connectionFactory.CreateConnection())
            {
                using (var channel = conn.CreateModel())
                {
                    channel.QueueDeclare(
                        queue: ServiceConstants.RabbitMQQueue,
                        durable: false,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null
                    );

                    var payload = JsonConvert.SerializeObject(@event);
                    var body = Encoding.UTF8.GetBytes(payload);

                    channel.BasicPublish(
                        exchange: "",
                        routingKey: ServiceConstants.RabbitMQQueue,
                        basicProperties: null,
                        body: body
                    );

                    _logger.LogInformation($"Emitted event {@event.GetType()}.");

                }
            }

            return Task.CompletedTask;
        }
    }
}
