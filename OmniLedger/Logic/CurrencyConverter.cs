using System;

namespace OmniLedger.Logic
{
    public static class CurrencyConverter
    {
        // Base currency is USD (1.0)
        private static decimal GetRate(string currencySymbol)
        {
            switch (currencySymbol.Trim())
            {
                case "$": return 1.0m;
                case "€": return 0.92m;
                case "£": return 0.79m;
                case "¥": return 150.0m;
                case "₱": return 56.0m;
                case "₹": return 83.0m;
                default: return 1.0m;
            }
        }

        public static decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            if (fromCurrency == toCurrency) return amount;

            decimal fromRate = GetRate(fromCurrency);
            decimal toRate = GetRate(toCurrency);

            if (fromRate == 0) fromRate = 1.0m;

            // Convert to base (USD), then to target
            decimal amountInUSD = amount / fromRate;
            decimal converted = amountInUSD * toRate;
            return Math.Round(converted, 2, MidpointRounding.AwayFromZero);
        }
    }
}
