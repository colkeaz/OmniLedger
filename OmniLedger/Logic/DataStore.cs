using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Globalization;

namespace OmniLedger.Logic
{
    public class DataStore
    {
        private string GetFilePath(string username) => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{username}_ledger.csv");

        public List<Transaction> LoadTransactions(string username)
        {
            var transactions = new List<Transaction>();
            var path = GetFilePath(username);

            if (!File.Exists(path)) return transactions;

            var lines = File.ReadAllLines(path).Skip(1); // Skip header
            foreach (var line in lines)
            {
                // Simple CSV parsing, assume no commas in descriptions for now since EscapeCSV implementation below avoids splitting correctly if we just use string.Split
                
                string[] parts = SplitCsvLine(line);
                if (parts.Length < 6) continue;

                int id = int.Parse(parts[0]);
                string type = parts[1];
                DateTime date = DateTime.Parse(parts[2], CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
                decimal amount = decimal.Parse(parts[3], CultureInfo.InvariantCulture);
                string desc = parts[4];
                string extra = parts[5];

                if (type == "Income")
                {
                    var income = new IncomeRecord(amount, extra, desc) { TransactionID = id, Date = date };
                    transactions.Add(income);
                }
                else if (type == "Expense")
                {
                    var expense = new BusinessExpense(amount, extra, desc) { TransactionID = id, Date = date };
                    transactions.Add(expense);
                }
            }

            return transactions;
        }

        public void SaveTransactions(string username, IEnumerable<Transaction> transactions)
        {
            var path = GetFilePath(username);
            var lines = new List<string> { "TransactionID,Type,Date,Amount,Description,ExtraField" };

            foreach (var t in transactions)
            {
                string extra = t is IncomeRecord inc ? inc.Source : (t is BusinessExpense exp ? exp.Category : "");
                string type = t is IncomeRecord ? "Income" : "Expense";
                lines.Add($"{t.TransactionID},{type},{t.Date.ToString("O", CultureInfo.InvariantCulture)},{t.Amount.ToString(CultureInfo.InvariantCulture)},{EscapeCSV(t.Description)},{EscapeCSV(extra)}");
            }

            File.WriteAllLines(path, lines);
        }

        private string EscapeCSV(string field)
        {
            if (string.IsNullOrEmpty(field)) return "";
            // Replace commas with semicolons to keep it simple and avoid complex CSV parsing
            return field.Replace(",", ";");
        }
        
        private string[] SplitCsvLine(string line)
        {
            return line.Split(',');
        }
    }
}
