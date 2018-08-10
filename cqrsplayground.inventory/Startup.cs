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
using System.Linq;

namespace cqrsplayground.gateway
{
    public class Startup
    {
        public IConfiguration Configuration { get; private set; }

        public void ConfigureServices(IServiceCollection services)
        {
            Configuration = (IConfiguration)services.First(service => service.ServiceType == typeof(IConfiguration)).ImplementationInstance;

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
            services.AddSingleton(Configuration);
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
