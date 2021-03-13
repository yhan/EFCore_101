using System;
using System.Threading;
using System.Threading.Tasks;
using System.Transactions;
using NFluent;
using NUnit.Framework;

namespace ConsoleApp.EF.Tests
{
    public class SharedDbContextConcurrencyIssue
    {
        //Shared DbContext simulate [IntegrationTestBase<T>](https://github.com/nexkap/FinexpayMain/blob/develop/Shared/Finexpay.Shared.DataIntegrationTests/Common/IntegrationTestBase.cs#L13)
        private readonly MyContext _context = DbHelper.CreateMyContext();
        private readonly ManualResetEventSlim _triggerRead = new(false);
        private readonly ManualResetEventSlim _releaseWrite = new(false);

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
        public async Task Shared_context_has_concurrency_problem()
        {
            Task.Run(() =>
            {
                var readTriggeredByWriteEnding = _triggerRead.Wait(TimeSpan.FromMilliseconds(500));
                Check.That(readTriggeredByWriteEnding).IsTrue();
                using var scope = StartNew();
                Check.ThatCode(() =>
                    {
                        foreach (var order in _context.Orders)
                        {
                            Console.WriteLine(order.OrderID);
                        }
                    })
                    .Throws<InvalidOperationException>()
                    .WithMessage("Connection is already attached to distributed transaction.");
                _releaseWrite.Set();
            }).Forget();

            using (var scope = StartNew())
            {
                await _context.Orders.AddAsync(new Order(43, 1, DateTime.Today));
                await _context.SaveChangesAsync();

                _triggerRead.Set();
                var released = _releaseWrite.Wait(TimeSpan.FromMilliseconds(500));
                Check.That(released).IsTrue();
            }
        }

        // Copy pasted from https://github.com/nexkap/FinexpayMain/blob/develop/Shared/Finexpay.Shared.DataIntegrationTests/Common/IsolatedAttribute.cs
        private static TransactionScope StartNew()
        {
            return new(TransactionScopeOption.Required,//RequiresNew has the same behaviour
                new TransactionOptions
                {
                    //Why setting tiemout to maximum value : https://blogs.msdn.microsoft.com/dbrowne/2010/06/03/using-new-transactionscope-considered-harmful/
                    Timeout = TransactionManager.MaximumTimeout,
                    //Understanding Isolation Levels : https://www.c-sharpcorner.com/article/understanding-isolation-levels/
                    IsolationLevel = IsolationLevel.ReadUncommitted
                },
                TransactionScopeAsyncFlowOption.Enabled);
        }
    }
}