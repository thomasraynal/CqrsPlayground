using cqrsplayground.shared;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public class RabbitMQConnectionFactory : ConnectionFactory
    {
        private ILogger<RabbitMQConnectionFactory> _logger;

        public RabbitMQConnectionFactory(ILoggerFactory loggerFactory, IOptions<RabbitMQConfiguration> conf)
        {
            _logger = loggerFactory.CreateLogger<RabbitMQConnectionFactory>();

            var configuration = conf.Value;

            this.UserName = configuration.Username;
            this.Password = configuration.Password;
            this.VirtualHost = configuration.VirtualHost;
            this.HostName = configuration.HostName;
            this.Uri = new Uri(configuration.Uri);

            _logger.LogInformation($"AMQP Connection configured for URI : {this.Uri}");
        }
    }
}
