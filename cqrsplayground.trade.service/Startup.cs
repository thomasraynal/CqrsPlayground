using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client.Events;
using Refit;
using Steeltoe.Discovery.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cqrsplayground.trade.service
{
    public class Startup: ServiceStartupBase
    {
        public Startup(IHostingEnvironment env): base(env)
        {
        }

        protected override void BuidConfigurationInternal(IConfigurationBuilder builder)
        {
            builder.AddJsonFile("servicesettings.json", optional: false, reloadOnChange: true);
        }

        protected override void ConfigureInternal(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDiscoveryClient();
        }

        protected override void ConfigureServicesInternal(IServiceCollection services)
        {
            services.AddSingleton<ITradeEventProcessor, TradeServiceEventProcessor>();
            services.AddHttpClientFor<ITradeService>(ServiceConstants.TradeRepositoryUrl);
            services.AddDiscoveryClient(Configuration);
        }


    }
}
