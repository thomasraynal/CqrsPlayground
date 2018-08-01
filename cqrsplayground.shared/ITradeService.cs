using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface ITradeService
    {
        Task Update(Trade trade);

        [Put("/trades")]
        Task<TradeCreationResult> Create([Body] TradeCreationDto tradeCreation);
        [Get("/trades/{tradeId}")]
        Task<Trade> Get(Guid tradeId);
        [Get("/trades")]
        Task<IEnumerable<Trade>> GetAll();
    }
}
