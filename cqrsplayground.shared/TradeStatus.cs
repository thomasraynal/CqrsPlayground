using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public enum TradeStatus
    {
        Created,
        Validated,
        Rejected,
        Booked
    }
}
