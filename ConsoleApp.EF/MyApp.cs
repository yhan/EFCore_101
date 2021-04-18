using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp.EF
{
    public class MyApp : IDisposable
    {
        private readonly MyContext _myContext;

        public MyApp(MyContext myContext)
        {
            _myContext = myContext;
        }

        public void Run()
        {
            var context = _myContext;
            var _offerIds = new List<Guid> {Guid.Empty, Guid.NewGuid()};

            var paymentValue = (from o in context.Offers.AsNoTracking()
                from r in o.Receivables
                from p in r.Payments
                where _offerIds.Contains(o.Id)
                group p by o.Id into paymentGroupsByOffer
                select new 
                {
                    OfferId = paymentGroupsByOffer.Key,
                    Sum = paymentGroupsByOffer.Select(p => p).Sum(p => p.Amount)
                }).ToDictionary(x => x.OfferId, x =>x.Sum);

            var receivableValue= (from o in context.Offers.AsNoTracking()
                from r in o.Receivables
                where _offerIds.Contains(o.Id)
                group r by o.Id
                into receivableGroupByOffer
                let recFlatten = receivableGroupByOffer
                select new
                {
                    OfferId = receivableGroupByOffer.Key,
                    Rec = recFlatten
                }).ToDictionary(x => x.OfferId,
                                x=> x.Rec );

            var offerMap = new Dictionary<Guid, decimal>();
            foreach (var a in receivableValue)
            {
                var valueOnPayments = paymentValue[a.Key];
                var receivables = a.Value;
                var initRdg = receivables.Select(r => r.InitialRdgAmountProducer2(r)?? 0M);

                var valueOnReceivable = receivables.Sum(r => r.FinancedAmount) - initRdg.Sum(r => r);
                offerMap.Add(a.Key, valueOnReceivable - valueOnPayments);
            }
            // rdg=  CommissionSale.IsValid() ? Math.Round(CommissionSale.RDG * FinancedAmount, 2) : (decimal?)null;
            // formula: receivable.FinancedAmount - (receivable.InitialRdgAmount ?? 0M) - payments.Sum(x => x.Amount);
        }

        public class Duo
        {
            public IEnumerable<CommissionSale> Comm { get; }
            public IEnumerable<decimal> FinancedAmount { get; }

            public Duo(IEnumerable<CommissionSale> comm, IEnumerable<decimal> financedAmount)
            {
                Comm = comm;
                FinancedAmount = financedAmount;
            }
        }

        private void IterateOrderDetails()
        {
            foreach (var detail in _myContext.OrderDetails)
            {
                Console.WriteLine(detail.Order.OrderID);
            }
        }

        public void Dispose()
        {
            _myContext?.Dispose();
        }
    }
}