using cqrsplayground.shared;
using Refit;
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

        public Task<TradeCreationResult> Create(TradeCreationDto tradeCreation)
        {
            var id = Guid.NewGuid();

            var trade = new Trade()
            {
                Asset = tradeCreation.Asset,
                Counterpary = tradeCreation.Counterpary,
                Currency = tradeCreation.Currency,
                Nominal = tradeCreation.Nominal,
                Price = tradeCreation.Price,
                Status = TradeStatus.None,
                Way = tradeCreation.Way,
                TradeId = id
            };

            _repository.AddOrUpdate(id, trade, (key, oldValue) => trade);

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

        public Task Update(Trade trade)
        {
            _repository.AddOrUpdate(trade.TradeId, trade, (key, oldValue) => trade);
            return Task.CompletedTask;
        }
    }
}
