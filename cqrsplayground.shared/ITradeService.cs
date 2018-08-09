using Refit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.shared
{
    public interface ITradeService
    {
        [Patch("/trades")]
        [Headers("Content-Type: application/json", "Realm: trade")]
        Task Update([Body] Trade trade);
        [Put("/trades")]
        [Headers("Realm: trade")]
        Task<TradeCreationResult> Create([Body] TradeCreationDto tradeCreation);
        [Get("/trades/{tradeId}")]
        [Headers("Realm: trade")]
        Task<Trade> Get(Guid tradeId);
        [Get("/trades")]
        [Headers("Realm: trade")]
        Task<IEnumerable<Trade>> GetAll();
    }
}
