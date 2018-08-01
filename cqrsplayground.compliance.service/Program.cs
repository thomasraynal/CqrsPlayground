using cqrsplayground.eventemitter;
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
               .UseUrls("http://locahost:5001")
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}
