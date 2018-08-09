using cqrsplayground.shared;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using Steeltoe.Discovery.Eureka.Transport;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace cqrsplayground.gateway
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("servicesettings.json", optional: false, reloadOnChange: true);

            this.Configuration = builder.Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddOptions()
                .AddMvc()
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.Converters.Add(new StringEnumConverter());
                    options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });

        
            services.AddTransient<IEurekaHttpClient, ServiceEurekaHttpClient>();
            services.AddTransient<IDiscoveryClient, EurekaDiscoveryClient>();
            services.AddSingleton<IConfiguration>(Configuration);
            services.AddSingleton<IInventoryService,InventoryClient>();
            services.AddSingleton<IAuthenticatedClientProvider, AuthenticatedClientProvider>();

            services.AddDiscoveryClient(Configuration);

            services.AddProxy();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole();
            loggerFactory.AddDebug();

            app.UseDiscoveryClient();
            app.UseMvc();

            app.RunProxy();
        }
    }
}
