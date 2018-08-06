﻿using cqrsplayground.eventemitter;
using cqrsplayground.shared;
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
               .UseUrls(ServiceConstants.BookingServiceUrl)
               .UseStartup<Startup>()
               .Build();

            host.Run();
        }
    }
}
