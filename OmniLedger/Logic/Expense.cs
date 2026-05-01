namespace OmniLedger.Logic
{
    /// <summary>
    /// Expense Record - inherits from Transaction
    /// Demonstrates polymorphism through method overriding
    /// </summary>
    public class BusinessExpense : Transaction
    {
        public string Category { get; set; } = "";

        public BusinessExpense() : base() { }

        public BusinessExpense(decimal amount, string category, string description = "")
        {
            Amount = amount;
            Category = category;
            Description = description;
            Date = System.DateTime.Now;
        }

        /// <summary>
        /// Polymorphic implementation: Expense formats differently than income
        /// </summary>
        public override string FormatRecord()
        {
            return $"Expense - {Category}: {Amount:C} on {Date:yyyy-MM-dd}";
        }

        public override string GetTransactionType() => "Expense";
    }
}