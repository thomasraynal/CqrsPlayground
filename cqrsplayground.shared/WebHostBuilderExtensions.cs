using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace cqrsplayground.shared
{
    public static class WebHostBuilderExtensions
    {
        public static IWebHostBuilder Setup(this IWebHostBuilder host, string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("commonsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("servicesettings.json", optional: true, reloadOnChange: true)
                .AddCommandLine(args)
                .Build();

            host.UseConfiguration(config);

            return host.UseUrls($"{config["scheme"]}://{config["host"]}:{config["port"]}");
        }
    }
}
