using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.EF
{
    class Program
    {
        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();
            var serviceProvider = ConfigureServices(config)
                .BuildServiceProvider();

            var context = new MyContext(serviceProvider);
            foreach (var orderDetail in context.OrderDetails)
            {

            }

            Console.WriteLine("Hello World!");
        }


        private static IServiceCollection ConfigureServices(IConfiguration config)
        {
            IServiceCollection services = new ServiceCollection();

            services.AddLogging(builder =>
            {
                builder.AddConsole();
                builder.AddConfiguration(config.GetSection("Logging"));
            });

            return services;
        }

        private static ILoggerFactory GetLoggerFactory()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddLogging(builder =>
                builder.AddConsole());

            return serviceCollection.BuildServiceProvider()
                .GetService<ILoggerFactory>();
        }

    }
}
