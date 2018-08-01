using cqrsplayground.shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public static class EventEmitterExtensions
    {

        public static IApplicationBuilder UseEventProcessor(this IApplicationBuilder app)
        {
            var processor = app.ApplicationServices.GetService<ITradeEventProcessor>();
            processor.Subscribe();
            return app;
        }

        public static IServiceCollection AddEventProcessor(this IServiceCollection services)
        {
            services.AddSingleton<EventingBasicConsumer, RabbitMQEventingConsumer>();
            services.AddSingleton<IConnectionFactory, RabbitMQConnectionFactory>();

            return services;
        }
    }
}
