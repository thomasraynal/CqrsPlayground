using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Steeltoe.Discovery.Client;
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

        protected override void BuidConfigurationInternal(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("servicesettings.json", optional: false, reloadOnChange: true);
        }

        protected override void ConfigureServicesInternal(IServiceCollection services)
        {
            services.AddSingleton<ITradeEventProcessor, TradeComplianceServiceEventProcessor>();
            services.AddDiscoveryClient(Configuration);
        }

        protected override void ConfigureInternal(IApplicationBuilder app, IHostingEnvironment env)
        { 
            app.UseDiscoveryClient();
        }
    }
}
