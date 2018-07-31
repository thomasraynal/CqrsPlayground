using System;

namespace cqrsplayground.shared
{
    public class TradeCreated : ITradeEvent
    {
        public Guid TradeId { get; set; }
        public string Type => typeof(TradeCreated).ToString();
    }
}
