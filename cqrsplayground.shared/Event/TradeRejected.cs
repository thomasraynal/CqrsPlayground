using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeRejected : ITradeEvent
    {
        public Guid TradeId { get; set; }
        public string Type => typeof(TradeRejected).ToString();
    }
}
