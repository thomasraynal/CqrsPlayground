using cqrsplayground.shared;
using cqrsplayground.trade.service;
using Microsoft.AspNetCore.Hosting;
using System;

namespace cqrsplayground.trade.repository
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
             .UseKestrel()
             .UseStartup<Startup>()
             .UseUrls(ServiceConstants.TradeRepositoryUrl)
             .Build();

            host.Run();
        }
    }
}
