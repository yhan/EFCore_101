using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ConsoleApp.EF.Tests
{
    public static class DbHelper
    {
        internal static MyContext CreateMyContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<MyContext>();
            optionsBuilder.UseLoggerFactory(GetLoggerFactory())
                .UsePostgreSql(Program.ConnectionString);

            return new MyContext(optionsBuilder.Options);
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