﻿using cqrsplayground.shared.Event;
using System;

namespace cqrsplayground.shared
{
    public class TradeCreated : EventBase
    {
        public override void Handle(Trade trade)
        {
            trade.Status = TradeStatus.Created;
        }
    }
}
