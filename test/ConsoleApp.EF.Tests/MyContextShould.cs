using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Query.ExpressionVisitors.Internal;
using NFluent;
using NUnit.Framework;

namespace ConsoleApp.EF.Tests
{
    public class MyContextShould
    {
        private MyContext _context = DbHelper.CreateMyContext();
        
        private ManualResetEventSlim _triggerRead = new ManualResetEventSlim(false);

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _context.Dispose();
        }

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            foreach (var order in _context.Orders)
            {
                _context.Orders.Remove(order);
            }
            _context.SaveChanges(true);
        }

        [Test]
        public async Task Shared_context_see_each_other()
        {
            Task.Run(async () =>
            {
                var triggered = _triggerRead.Wait(500);
                Console.WriteLine($"Trigger read successfully = {triggered}");
                using (var scope = StartNew())
                {
                    Console.WriteLine("Watching thread");

                    using var localContext = DbHelper.CreateMyContext();
                    Console.WriteLine($"**** Watching thread count orders: {localContext.Orders.Count()}");
                    foreach (var order in localContext.Orders)
                    {
                        Console.WriteLine(order.OrderID);
                    }
                }
            }).Forget();

            using (var scope = StartNew())
            {
                await _context.Orders.AddAsync(new Order(43, 1, DateTime.Today));
                await _context.SaveChangesAsync();
                Console.WriteLine("Writing thread");
                //scope.Complete();
                _triggerRead.Set();
            }

            Check.That(_context.Orders).HasSize(0);

            await Task.Delay(1000);
        }


        private static TransactionScope StartNew()
        {
            return new TransactionScope(TransactionScopeOption.Required,
                new TransactionOptions
                {
                    //Why setting tiemout to maximum value : https://blogs.msdn.microsoft.com/dbrowne/2010/06/03/using-new-transactionscope-considered-harmful/
                    Timeout = TransactionManager.MaximumTimeout,
                    //Understanding Isolation Levels : https://www.c-sharpcorner.com/article/understanding-isolation-levels/
                    IsolationLevel = IsolationLevel.Snapshot
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}