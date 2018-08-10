using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder Setup(this IWebHostBuilder webHost, string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("commonsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("servicesettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();


            config["eureka:client:serviceUrl"] = config["gateway"];

            var scheme = config["scheme"];
            var host = config["eureka:instance:hostName"] = config["host"];
            var port = config["eureka:instance:port"] = config["eureka:instance:nonSecurePort"] = config["port"];

            webHost.UseConfiguration(config);

            return webHost.UseUrls($"{scheme}://{host}:{port}");
        }
    }
}
