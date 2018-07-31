using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface ITradeEventEmitter
    {
        Task Emit(ITradeEvent @event);
    }
}
