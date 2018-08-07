using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading;

namespace cqrsplayground.trade.service
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
               .UseKestrel()
               .Setup(args)
               .UseStartup<Startup>()
               .Build();

            host.Run();

        }
    }
}
