using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.compliance
{
    public class Startup : ServiceStartupBase
    {
        public Startup(IHostingEnvironment env): base(env)
        {
        }

        protected override void ConfigureServicesInternal(IServiceCollection services)
        {
            services.AddSingleton<ITradeEventProcessor, TradeComplianceServiceEventProcessor>();
        }
    }
}
