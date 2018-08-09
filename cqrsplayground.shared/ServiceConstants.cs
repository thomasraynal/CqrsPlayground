using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    //refacto: inject hosting parameters via env variables
    public static class ServiceConstants
    {
        public static String TradeRepositoryUrl = "http://localhost:5003";
        public static String ServiceGatewayUrl = "http://localhost:5004";

        public static String RabbitMQConfig = "rabbitmq";
        public static String TradeEventService = "trade";
        public static String BookingEventService = "booking";
        public static String ComplianceEventService = "compliance";
        public static string EventExchange = "events";
    }
}
