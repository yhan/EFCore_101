using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ConsoleApp.EF
{
    public class Offer
    {
        public Offer(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }

        public Offer()
        {
            Receivables = new List<Receivable>();
        }

        public ICollection<Receivable> Receivables { get; set; }
    }

    
    public class Receivable
    {
        public Receivable()
        {
        }

        public Receivable(Guid id) 
        {
            Id = id;
        }

        public Guid Id { get; set; }

        [ForeignKey("Offer")]
        public Guid? Offer_Id { get; set; }

        public decimal FinancedAmount { get; set; }

        public virtual decimal? InitialRdgAmount =>
            //return CommissionSale.IsValid() ? Math.Round(CommissionSale.RDG * FinancedAmount, 2) : (decimal?)null;
            InitialRdgAmountProducer(this.CommissionSale, this.FinancedAmount);

        public static Func<CommissionSale, decimal, decimal?> InitialRdgAmountProducer 
            => (comm, financedAmount) => comm.IsValid() ? Math.Round(comm.RDG * financedAmount, 2) : default;

        public Func<Receivable, decimal?> InitialRdgAmountProducer2
            => r =>
            {
                var commissionSale = r.CommissionSale;
                return commissionSale.IsValid() ? Math.Round(commissionSale.RDG * r.FinancedAmount, 2) : default;
            };


        public decimal? SellerFailureScore { get; set; }
        public decimal? SellerAppliedFailureScore { get; set; }
        public decimal? DebtorFailureScore { get; set; }
        public decimal? DebtorAppliedFailureScore { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }

        // Another way to define complex type:
        // [Owned]
        public CommissionSale CommissionSale { get; set; }
    }

    public class CommissionSale
    {
        public int ScoreCedant { get; set; }
        public decimal CommissionCession { get; set; }
       
        public decimal CommissionSurCreanceEnArriere { get; set; }
        public int ArrearsHorizon { get; set; }
        public decimal RDG { get; set; }
       
        public decimal TheoriticalCommissionSale { get; set; }
        public decimal CommissionFK { get; set; }
        public decimal ExcessSpread { get; set; }
        public decimal RiskCost { get; set; }
       
        public decimal FundFees { get; set; }
        public decimal MinimumCommissionSale { get; set; }

        public bool Check1 { get; set; }

        public bool Check2 { get; set; }

        public bool IsValid()
        {
            return Check1 && Check2;
        }
    }

    public class Payment
    {
        public Payment() { }

        public Payment(Guid id) : this()
        {
            Id = id;
        }

        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}