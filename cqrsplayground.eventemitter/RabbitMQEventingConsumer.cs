using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public class RabbitMQEventingConsumer : EventingBasicConsumer
    {
        public RabbitMQEventingConsumer(IConnectionFactory factory) : base(factory.CreateConnection().CreateModel())
        {
        }
    }
}
