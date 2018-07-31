using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class Trade
    {
        public Guid TradeId { get; set; }
        public double Nominal { get; set; }
        public String Asset { get; set; }
        public String Currency { get; set; }
        public String Counterpary { get; set; }
        public double Price { get; set; }
        public String Book { get; set; }
        public TradeStatus Status { get; set; }
        public TradeWay Way { get; set; }
    }
}
