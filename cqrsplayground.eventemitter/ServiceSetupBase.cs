using cqrsplayground.shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public abstract class ServiceStartupBase
    {
        public IConfigurationRoot Configuration { get; }

        public ServiceStartupBase(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("commonsettings.json", optional: false, reloadOnChange: true);

            BuidConfigurationInternal(builder);

            this.Configuration = builder.Build();
        }

        protected virtual void BuidConfigurationInternal(IConfigurationBuilder services)
        {

        }

        protected virtual void ConfigureServicesInternal (IServiceCollection services)
        {
    
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();

            services
                .AddEventProcessor()
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            services.AddSingleton<IConfiguration>(Configuration);

            services.Configure<RabbitMQConfiguration>(Configuration.GetSection(ServiceConstants.RabbitMQConfig));

            ConfigureServicesInternal(services);

        }

        protected virtual void ConfigureInternal(IApplicationBuilder app, IHostingEnvironment env)
        {

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseMvc();

            app.UseEventProcessor();

            ConfigureInternal(app, env);
        }
    }
}
