using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public class TradeCreationDemand
    {
        public double Nominal { get; set; }
        public String Asset { get; set; }
        public String Currency { get; set; }
        public String Counterpary { get; set; }
        public TradeWay Way { get; set; }
        public double Price { get; set; }

        public override string ToString()
        {
            return $"{Asset} - {Way} [{Counterpary} {Price} ({Currency}) {Nominal}]";
        }
    }
}
