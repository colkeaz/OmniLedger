namespace OmniLedger.Logic
{
    public class Income : Transaction
    {
        public string Source { get; set; }
        public override string GetTransactionDetails() => $"[INCOME] {Source}: {Amount:C}";
    }
}