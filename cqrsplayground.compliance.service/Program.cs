using cqrsplayground.eventemitter;
using Microsoft.AspNetCore.Hosting;
using System;

namespace cqrsplayground.compliance.service
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
               .UseKestrel()
               .UseStartup<Startup>()
               .UseEventEmitter()
               .Build();

            host.Run();
        }
    }
}
