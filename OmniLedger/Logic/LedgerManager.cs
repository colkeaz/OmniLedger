using System.Collections.Generic;

namespace OmniLedger.Logic
{
    public class LedgerManager
    {
        private decimal _currentBalance;
        public decimal CurrentBalance => _currentBalance;

        public bool ValidateFunds(decimal amount) => (_currentBalance - amount) >= 0;

        public void ProcessTransaction(Transaction t)
        {
            if (t is Income) _currentBalance += t.Amount;
            else if (t is Expense && ValidateFunds(t.Amount)) _currentBalance -= t.Amount;
        }
    }
}