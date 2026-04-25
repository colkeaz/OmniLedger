namespace OmniLedger.Logic
{
    public class Expense : Transaction
    {
        public string Category { get; set; }
        public override string GetTransactionDetails() => $"[EXPENSE] {Category}: {Amount:C2}";
    }
}