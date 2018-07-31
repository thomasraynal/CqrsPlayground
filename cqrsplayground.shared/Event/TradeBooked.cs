using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeBooked : ITradeEvent
    {
        public Guid TradeId { get; set; }
        public String TradeBook { get; set; }
        public string Type => typeof(TradeBooked).ToString();
    }
}
