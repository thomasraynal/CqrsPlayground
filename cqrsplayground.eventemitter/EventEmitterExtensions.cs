using cqrsplayground.shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public static class EventEmitterExtensions
    {
        public static IWebHostBuilder UseEventEmitter(this IWebHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((services) =>
            {
                services.AddSingleton<ITradeEventEmitter, RabbitMQEventEmitter>();
                services.AddSingleton<ITradeEventProcessor, RabbitMQEventProcessor>();
                services.AddSingleton<EventingBasicConsumer, RabbitMQEventingConsumer>();
            });

            return hostBuilder;
        }
    }
}
