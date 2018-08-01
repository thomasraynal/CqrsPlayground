using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeUpdateDto
    {
        public Guid TradeId { get; set; }
        public String Book { get; set; }
        public TradeStatus Status { get; set; }
    }
}
