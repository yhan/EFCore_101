using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using Microsoft.Extensions.Options;

namespace ConsoleApp.EF
{
    public class Program
    {
        private const string DbName = "StoreDB";
        private const string Schema = "public";

        // Min Pool Size=0;Max Pool Size=100;
        public static readonly string ConnectionString = $"User Id=postgres;Password=root;Host=localhost;Database={DbName};Port=5433;Persist Security Info=True;Initial Schema={Schema};Unicode=True;License Key=wMnQAqtt0hxxsDqJWvW9KxIj97Gok/73AlbVrLT0DD/uMFfe0qmajvZ7hwGLdgDsU0YXAbKCsQdefn/Tk2w4hP6ayVO14kefgaCTThN0Pserh6XISmNJiiEYrDxw/mlJfIbmJZovZts6ABoyLNOe24T/xWFPBVcL/dA8irfcBzxMXYEdH/IHfWenC2oN4DOh7lPTxS8zLJP7KXn1u85/8B6VoQtWnxcwDcLSKYfXHZGoVidjLYm8LiWYB7hTGU3dgx0AQsbDJN1CVVvXyC9BA/mWxvV+LRnXRYzr5jTW50U=";

        static void Main(string[] args)
        {
            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", true, true)
                .Build();

            var services = new ServiceCollection();
            var svcProvider = services.AddLogging(builder =>
                {
                    builder.AddConsole();
                    builder.AddConfiguration(config.GetSection("Logging"));
                })
                .AddDbContext<MyContext>(ob =>
                {
                    var optionsMonitor = services.BuildServiceProvider().GetService<IOptionsMonitor<ConsoleLoggerOptions>>();

                    ob.UseLoggerFactory(new LoggerFactory(new[]
                        {
                            new ConsoleLoggerProvider(optionsMonitor)
                        }))
                        .UsePostgreSql(ConnectionString);
                })
                .AddTransient<MyApp>()
                .BuildServiceProvider();


            svcProvider.GetService<MyApp>().Run();
            

            Console.WriteLine("Hello World!");
        }
        
    }
}
