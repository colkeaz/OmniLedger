using System;
using System.Collections.Generic;
using System.Linq;

namespace OmniLedger.Logic
{
    /// <summary>
    /// LedgerManager - Manages all financial transactions
    /// Encapsulates private balance and transaction history
    /// Provides secure, validated methods for transaction processing
    /// </summary>
    public class LedgerManager
    {
        private decimal _currentBalance = 0;
        private List<Transaction> _transactionHistory = new List<Transaction>();
        private int _transactionCounter = 1000;
        
        private string _username;
        private DataStore _dataStore;
        private string _currentCurrencySymbol = "$";
        private readonly object _syncRoot = new object();

        public decimal CurrentBalance => _currentBalance;
        public string CurrentCurrencySymbol => _currentCurrencySymbol;
        public IReadOnlyList<Transaction> TransactionHistory => _transactionHistory.AsReadOnly();

        public LedgerManager(string username, string initialCurrency = "$")
        {
            _username = username;
            _currentCurrencySymbol = initialCurrency;
            _dataStore = new DataStore();
            _transactionHistory = _dataStore.LoadTransactions(_username);
            
            if (_transactionHistory.Count > 0)
            {
                _transactionCounter = _transactionHistory.Max(t => t.TransactionID);
                _currentBalance = _transactionHistory.Sum(t => t is IncomeRecord ? t.Amount : -t.Amount);
            }
        }

        /// <summary>
        /// Change the ledger currency and convert all existing transactions
        /// Encapsulation ensures that balance updates are synchronized with currency changes
        /// </summary>
        public void ChangeCurrency(string newCurrency)
        {
            if (string.IsNullOrEmpty(newCurrency) || newCurrency == _currentCurrencySymbol)
                return;

            lock (_syncRoot)
            {
                string oldCurrency = _currentCurrencySymbol;
                _currentCurrencySymbol = newCurrency;

                foreach (var transaction in _transactionHistory)
                {
                    transaction.Amount = CurrencyConverter.Convert(transaction.Amount, oldCurrency, newCurrency);
                }

                // Recalculate balance to ensure precision
                _currentBalance = _transactionHistory.Sum(t => t is IncomeRecord ? t.Amount : -t.Amount);
                _dataStore.SaveTransactions(_username, _transactionHistory);
            }
        }

        /// <summary>
        /// Validates if sufficient funds exist for an expense
        /// </summary>
        public bool ValidateFunds(decimal amount) => (_currentBalance - amount) >= 0;

        /// <summary>
        /// Secure method to process transactions - validates and updates balance
        /// Encapsulation ensures data integrity
        /// </summary>
        public bool ProcessTransaction(Transaction transaction)
        {
            if (transaction == null)
                throw new ArgumentNullException(nameof(transaction));

            if (transaction.Amount <= 0)
                throw new ArgumentException("Transaction amount must be positive");

            lock (_syncRoot)
            {
                // Assign unique transaction ID
                transaction.TransactionID = ++_transactionCounter;
                transaction.Date = DateTime.Now;

                // Process income
                if (transaction is IncomeRecord)
                {
                    _currentBalance += transaction.Amount;
                    _transactionHistory.Add(transaction);
                    _dataStore.SaveTransactions(_username, _transactionHistory);
                    return true;
                }

                // Process expense with fund validation
                if (transaction is BusinessExpense)
                {
                    if (!ValidateFunds(transaction.Amount))
                        return false; // Prevent overdraft

                    _currentBalance -= transaction.Amount;
                    _transactionHistory.Add(transaction);
                    _dataStore.SaveTransactions(_username, _transactionHistory);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Get all transactions with optional filtering
        /// </summary>
        public List<Transaction> GetAllTransactions() => new List<Transaction>(_transactionHistory);

        public List<IncomeRecord> GetIncomeTransactions() 
            => _transactionHistory.OfType<IncomeRecord>().ToList();

        public List<BusinessExpense> GetExpenseTransactions() 
            => _transactionHistory.OfType<BusinessExpense>().ToList();

        /// <summary>
        /// Get transactions for a specific date range
        /// </summary>
        public List<Transaction> GetTransactionsByDateRange(DateTime startDate, DateTime endDate)
            => _transactionHistory.Where(t => t.Date >= startDate && t.Date <= endDate).ToList();

        /// <summary>
        /// Get summary statistics
        /// </summary>
        public decimal GetTotalIncome() 
            => _transactionHistory.OfType<IncomeRecord>().Sum(t => t.Amount);

        public decimal GetTotalExpenses() 
            => _transactionHistory.OfType<BusinessExpense>().Sum(t => t.Amount);

        /// <summary>
        /// Undo the last transaction (for demonstration)
        /// </summary>
        public bool UndoLastTransaction()
        {
            lock (_syncRoot)
            {
                if (_transactionHistory.Count == 0)
                    return false;

                Transaction lastTransaction = _transactionHistory[_transactionHistory.Count - 1];
                
                if (lastTransaction is IncomeRecord)
                    _currentBalance -= lastTransaction.Amount;
                else if (lastTransaction is BusinessExpense)
                    _currentBalance += lastTransaction.Amount;

                _transactionHistory.RemoveAt(_transactionHistory.Count - 1);
                _dataStore.SaveTransactions(_username, _transactionHistory);
                return true;
            }
        }
    }
}