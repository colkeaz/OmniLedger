using System;
using System.Collections.Generic;

namespace OmniLedger.Logic
{
    public static class CurrencyConverter
    {
        private static readonly HashSet<string> ValidSymbols = new HashSet<string> { "$", "€", "£", "¥", "₱", "₹" };

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

        /// <summary>
        /// Returns true if the symbol is a recognized currency
        /// </summary>
        public static bool IsValidCurrency(string symbol)
        {
            return !string.IsNullOrEmpty(symbol) && ValidSymbols.Contains(symbol.Trim());
        }

        /// <summary>
        /// Returns the symbol if valid, or "$" as a safe fallback
        /// </summary>
        public static string SanitizeCurrency(string symbol)
        {
            return IsValidCurrency(symbol) ? symbol.Trim() : "$";
        }

        public static decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            fromCurrency = SanitizeCurrency(fromCurrency);
            toCurrency = SanitizeCurrency(toCurrency);

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
