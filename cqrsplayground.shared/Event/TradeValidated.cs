using cqrsplayground.shared.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeValidated : EventBase
    {
        public override void Handle(Trade trade)
        {
            trade.Status = TradeStatus.Validated;
        }
    }
}
