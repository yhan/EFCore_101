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

        public void Run2()
        {
            var context = _myContext;
            var _offerIds = new List<Guid> {Guid.Empty, Guid.NewGuid()};

            var paymentValue = (from r in context.Receivables.AsNoTracking()
                
                from p in r.Payments
                where r.Offer_Id.HasValue && _offerIds.Contains(r.Offer_Id.Value)
                group p by r.Offer_Id.Value into paymentGroupsByOffer
                select new 
                {
                    OfferId = paymentGroupsByOffer.Key,
                    Sum = paymentGroupsByOffer.Select(p => p).Sum(p => p.Amount)
                }).ToDictionary(x => x.OfferId, x =>x.Sum);

            var receivableValue= ( 
                from r in context.Receivables.AsNoTracking()
                where r.Offer_Id.HasValue && _offerIds.Contains(r.Offer_Id.Value)
                group r by r.Offer_Id.Value
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


        public void Run()
        {
            var context = _myContext;
            var _offerIds = new List<Guid> {Guid.Empty, Guid.NewGuid()};

            
            var paymentValue = (from r in context.Receivables.AsNoTracking()
                
                from p in r.Payments
                where r.Offer_Id.HasValue && _offerIds.Contains(r.Offer_Id.Value)
                group p by r.Offer_Id.Value into paymentGroupsByOffer
                select new 
                {
                    OfferId = paymentGroupsByOffer.Key,
                    Sum = paymentGroupsByOffer.Select(p => p).Sum(p => p.Amount)
                }).ToDictionary(x => x.OfferId, x =>x.Sum);

            var receivableValue= (
                from r in context.Receivables.AsNoTracking()
                where r.Offer_Id.HasValue && _offerIds.Contains(r.Offer_Id.Value)
                group r by r.Offer_Id.Value
                into receivableGroupByOffer
                let recFlatten = receivableGroupByOffer
                select new
                {
                    OfferId = receivableGroupByOffer.Key,
                    FinancementSum = recFlatten.Sum(r=> r.FinancedAmount),
                    RdgCalcSource = recFlatten.Select(r =>new RdgCalcSource(r.CommissionSale, r.FinancedAmount) )
                }).ToDictionary(x => x.OfferId,
                                x=> new { x.RdgCalcSource, x.FinancementSum } );

            var offerMap = new Dictionary<Guid, decimal>();
            foreach (var a in receivableValue)
            {
                var valueOnPayments = paymentValue[a.Key];

                decimal rdg = 0M;
                var duos = a.Value;
                foreach (var duo in duos.RdgCalcSource)
                {
                    var initRdg = Receivable.InitialRdgAmountProducer(duo.CommissionSale, duo.FinancedAmount);
                    rdg += initRdg ?? 0M;
                }

                var valueOnReceivable = duos.FinancementSum - rdg;

                offerMap.Add(a.Key, valueOnReceivable - valueOnPayments);
            }
            // rdg=  CommissionSale.IsValid() ? Math.Round(CommissionSale.RDG * FinancedAmount, 2) : (decimal?)null;
            // formula: receivable.FinancedAmount - (receivable.InitialRdgAmount ?? 0M) - payments.Sum(x => x.Amount);
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

    public class RdgCalcSource
    {
        public CommissionSale CommissionSale { get; }
        public decimal FinancedAmount { get; }

        public RdgCalcSource(CommissionSale commissionSale, decimal financedAmount)
        {
            CommissionSale = commissionSale;
            FinancedAmount = financedAmount;
        }
    }
}