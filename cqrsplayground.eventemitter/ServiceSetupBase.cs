using cqrsplayground.authentication;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace cqrsplayground.eventemitter
{
    public abstract class ServiceStartupBase
    {
        public IConfiguration Configuration { get; private set; }

        protected virtual void ConfigureServicesInternal(IServiceCollection services)
        {

        }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = (IConfiguration)services.First(service => service.ServiceType == typeof(IConfiguration)).ImplementationInstance;

            services
                .AddOptions()
                .AddEventProcessor()
                .AddAuthenticationWorkflow(Configuration)
                .Configure<RabbitMQConfiguration>(Configuration.GetSection(ServiceConstants.RabbitMQConfig))
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

            ConfigureServicesInternal(services);

        }

        protected virtual void ConfigureInternal(IApplicationBuilder app, IHostingEnvironment env)
        {

        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            ConfigureInternal(app, env);

            app
                .AddExceptionHandler()
                .UseMvc()
                .UseEventProcessor();
        }
    }
}
