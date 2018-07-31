using cqrsplayground.shared;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.trade.service
{
    public class InMemoryTradeCache : ITradeService
    {
        private ConcurrentDictionary<Guid, Trade> _repository;

        public InMemoryTradeCache()
        {
            _repository = new ConcurrentDictionary<Guid, Trade>();
        }

        public Task<TradeCreationResult> Create(TradeCreationDemand tradeCreationDemand)
        {
            var id = Guid.NewGuid();

            _repository.AddOrUpdate(id, new Trade()
            {
                Asset = tradeCreationDemand.Asset,
                Counterpary = tradeCreationDemand.Counterpary,
                Currency = tradeCreationDemand.Currency,
                Nominal = tradeCreationDemand.Nominal,
                Price = tradeCreationDemand.Price,
                Status = TradeStatus.Created,
                Way = tradeCreationDemand.Way,
                TradeId = id

            }, null);

            return Task.FromResult(new TradeCreationResult() { TradeId = id });
        }

        public Task<Trade> Get(Guid tradeId)
        {
            return Task.FromResult(_repository[tradeId]);
        }

        public Task<IEnumerable<Trade>> GetAll()
        {
            return Task.FromResult(_repository.Values.AsEnumerable());
        }
    }
}
