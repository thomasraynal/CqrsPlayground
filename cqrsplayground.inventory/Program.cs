using Microsoft.AspNetCore.Hosting;
using System;

namespace cqrsplayground.inventory
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new WebHostBuilder()
              .UseKestrel()
              .UseUrls("http://locahost:5005")
              .UseStartup<Startup>()
              .Build();

            host.Run();
        }
    }
}
