using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;

namespace cqrsplayground.trade.service
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
               .UseKestrel()
               .UseUrls(ServiceConstants.TradeServiceUrl)
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}
