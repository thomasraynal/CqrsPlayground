using cqrsplayground.eventemitter;
using cqrsplayground.shared;
using Microsoft.AspNetCore.Hosting;
using System;

namespace cqrsplayground.compliance
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
