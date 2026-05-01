namespace OmniLedger.Logic
{
    /// <summary>
    /// Income Record - inherits from Transaction
    /// Demonstrates polymorphism through method overriding
    /// </summary>
    public class IncomeRecord : Transaction
    {
        public string Source { get; set; } = "";

        public IncomeRecord() : base() { }

        public IncomeRecord(decimal amount, string source, string description = "")
        {
            Amount = amount;
            Source = source;
            Description = description;
            Date = System.DateTime.Now;
        }

        /// <summary>
        /// Polymorphic implementation: Income formats differently than expenses
        /// </summary>
        public override string FormatRecord()
        {
            return $"Income - {Source}: {Amount:C} on {Date:yyyy-MM-dd}";
        }

        public override string GetTransactionType() => "Income";
    }
}