using cqrsplayground.eventemitter;
using Microsoft.AspNetCore.Hosting;
using System;

namespace cqrsplayground.booking.service
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
