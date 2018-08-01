using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface ITradeEventProcessor
    {
        IEnumerable<Type> AllowedEvents { get; }
        Task OnEvent(ITradeEvent @event);
        Task Emit(ITradeEvent @event);
        void Subscribe();
        void Unsubscribe();
    }
}
