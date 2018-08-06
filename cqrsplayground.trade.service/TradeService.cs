using cqrsplayground.authentication;
using cqrsplayground.authentication.service;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Refit;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Authorize = Microsoft.AspNetCore.Authorization.AuthorizeAttribute;

namespace cqrsplayground.trade.service
{
    [Route("")]
    public class TradeServiceAuth : AuthentificationService
    {
        public TradeServiceAuth(IAuthenticationService auth, IServiceInstance instance) : base(auth, instance)
        {
        }
    }

    [Route("trades")]
    public class TradeService : ControllerBase, ITradeService
    {
        private ILogger<TradeService> _logger;
        private ITradeEventProcessor _tradeEventProcessor;
        private ITradeService _repository;
        private IAuthenticationService _authenticationService;
        private IConfiguration _configuration;
        private IServiceInstance _instance;

        public TradeService(
            ILoggerFactory loggerFactory, 
            ITradeService repository, 
            IConfiguration configuration, 
            IAuthenticationService auth,
            IServiceInstance instance,
            ITradeEventProcessor tradeEventProcessor)
        {
            _logger = loggerFactory.CreateLogger<TradeService>();
            _tradeEventProcessor = tradeEventProcessor;
            _repository = repository;
            _authenticationService = auth;
            _configuration = configuration;
            _instance = instance;
        }

        [HttpPut]
        [JwtBearerAuthorize]
        public async Task<TradeCreationResult> Create([FromBody] TradeCreationDto tradeCreationDemand)
        {

            var result = await _repository.Create(tradeCreationDemand);

            var @event = new TradeCreated()
            {
                TradeId = result.TradeId
            };

            await _tradeEventProcessor.Emit(@event);

            return result;

        }

        [HttpGet("{tradeId}")]
        [AllowAnonymous]
        public async Task<Trade> Get(Guid tradeId)
        {
            return await _repository.Get(tradeId); 
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IEnumerable<Trade>> GetAll()
        {
            return await _repository.GetAll();
        }

        public Task Update([FromBody] Trade trade)
        {
            throw new NotImplementedException();
        }
    }
}
