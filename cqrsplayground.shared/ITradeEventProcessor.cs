using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface ITradeEventProcessor
    {
        IObservable<ITradeEvent> OnNewEvent { get; }
    }
}
