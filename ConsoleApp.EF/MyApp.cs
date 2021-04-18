using System;
using System.Collections.Generic;
using System.Linq;

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
            var myContext = new MyContext();
            var offerIds = new List<Guid>();

            var paymentValue = (from o in myContext.Offers
                from r in o.Receivables
                from p in r.Payments
                where offerIds.Contains(o.Id)
                group p by o.Id into paymentGroupsByOffer
                select new 
                {
                    OfferId = paymentGroupsByOffer.Key,
                    Sum = paymentGroupsByOffer.Select(p => p).Sum(p => p.Amount)
                }).ToDictionary(x => x.OfferId, x =>x.Sum);

            var receivableValue= (from o in myContext.Offers
                from r in o.Receivables
                where offerIds.Contains(o.Id)
                group r by o.Id
                into receivableGroupByOffer
                select new
                {
                    OfferId = receivableGroupByOffer.Key,
                    Calc = receivableGroupByOffer.Select(r => r).Sum(r => r.FinancedAmount - r.InitialRdgAmount)
                }).ToDictionary(x => x.OfferId, x=> x.Calc);

            var offerMap = new Dictionary<Guid, decimal>();
            foreach (var a in receivableValue)
            {
                var b = paymentValue[a.Key];
                offerMap.Add(a.Key, b - a.Value);
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