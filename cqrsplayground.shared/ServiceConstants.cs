using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public static class ServiceConstants
    {
        public static String TradeServiceUrl = "http://localhost:5000";
        //public static String TradeEventServiceUrl = "http://localhost:5672";
        //public static String RabbitMQUri = "amqp://localhost:5672/";

        //public static String RabbitMQComplianceQueue = "compliance";
        //public static String RabbitMQBookingQueue = "booking";

        public static String RabbitMQConfig = "rabbitmq";
            
        public static String TradeEventService = "trade";
        public static String BookingEventService = "booking";
        public static String ComplianceEventService = "compliance";

        public static string EventExchange = "events";

        //public static String TradeServiceUrl = "http://trades";
        //public static String TradeEventServiceUrl = "http://trade-event:5672";
        //public static String RabbitMQUri = "amqp://trade-event:5672/";
        //public static String RabbitMQQueue = "trades";
    }
}
