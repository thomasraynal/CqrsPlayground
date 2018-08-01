using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared.Event
{
    public abstract class EventBase : ITradeEvent
    {
        public Guid TradeId { get; set; }

        public string Type => GetType().AssemblyQualifiedName;

        public abstract void Handle(Trade trade);
    }
}
