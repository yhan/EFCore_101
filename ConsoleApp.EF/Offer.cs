using System;
using System.Collections.Generic;

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

        public decimal FinancedAmount { get; set; }
        public decimal InitialRdgAmount { get; set; }

        public decimal? SellerFailureScore { get; set; }
        public decimal? SellerAppliedFailureScore { get; set; }
        public decimal? DebtorFailureScore { get; set; }
        public decimal? DebtorAppliedFailureScore { get; set; }

        public virtual ICollection<Payment> Payments { get; set; }
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