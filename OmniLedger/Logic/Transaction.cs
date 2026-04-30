using System;

namespace OmniLedger.Logic
{
    public abstract class Transaction
    {
        public int TransactionID { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; }

        public abstract string GetTransactionDetails();
    }
}