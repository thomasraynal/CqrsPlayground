using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeValidated : ITradeEvent
    {
        public Guid TradeId { get; set; }
        public Guid ComplianceCheckId { get; set; }
        public string Type => typeof(TradeValidated).ToString();
    }
}
