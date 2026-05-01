using System;

namespace OmniLedger.Logic
{
    /// <summary>
    /// Base Transaction class - demonstrates inheritance and encapsulation
    /// All financial records inherit from this class to maintain uniform ledger structure
    /// </summary>
    public abstract class Transaction
    {
        private int _transactionID;
        private DateTime _date;
        private decimal _amount;
        private string _description;

        // Encapsulation: Properties with private backing fields
        public int TransactionID 
        { 
            get { return _transactionID; } 
            set { _transactionID = value; } 
        }

        public DateTime Date 
        { 
            get { return _date; } 
            set { _date = value; } 
        }

        public decimal Amount 
        { 
            get { return _amount; } 
            set 
            { 
                if (value < 0) throw new ArgumentException("Amount cannot be negative");
                _amount = value; 
            } 
        }

        public string Description 
        { 
            get { return _description; } 
            set { _description = value ?? ""; } 
        }

        public Transaction()
        {
            _date = DateTime.Now;
            _transactionID = 0;
            _amount = 0;
            _description = "";
        }

        // Polymorphism: Abstract method - different behavior for Income vs Expense
        public abstract string FormatRecord();
        
        public abstract string GetTransactionType();
    }
}