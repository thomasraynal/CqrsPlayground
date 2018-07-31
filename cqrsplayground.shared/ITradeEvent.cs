using System;

namespace cqrsplayground.shared
{
    public interface ITradeEvent
    {
         Guid TradeId { get; set; }
         String Type { get; }
    }
}