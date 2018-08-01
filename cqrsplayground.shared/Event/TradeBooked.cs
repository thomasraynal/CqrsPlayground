using cqrsplayground.shared.Event;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeBooked : EventBase
    {
        public String TradeBook { get; set; }

        public override void Handle(Trade trade)
        {
            trade.Status = TradeStatus.Booked;
            trade.Book = TradeBook;
        }
    }
}
