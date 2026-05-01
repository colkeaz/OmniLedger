using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OmniLedger.Logic
{
    /// <summary>
    /// ExcelExporter - Implementation of IReportGenerator
    /// Exports financial data to Excel format (.xlsx)
    /// </summary>
    public class ExcelExporter : IReportGenerator
    {
        public string GetFileExtension() => ".csv";

        public void GenerateReport(List<Transaction> transactions, decimal totalBalance, string filePath)
        {
            try
            {
                // Create a CSV file (Excel compatible)
                StringBuilder sb = new StringBuilder();

                // Header
                sb.AppendLine("OmniLedger Financial Report");
                sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine();
                sb.AppendLine($"Total Balance,{totalBalance:C}");
                sb.AppendLine();

                // Transaction headers
                sb.AppendLine("Transaction ID,Date,Type,Description,Amount,Running Balance");

                decimal runningBalance = 0;

                // Transaction data
                foreach (var transaction in transactions)
                {
                    if (transaction is IncomeRecord income)
                    {
                        runningBalance += transaction.Amount;
                        sb.AppendLine($"{transaction.TransactionID},{transaction.Date:yyyy-MM-dd},{income.GetTransactionType()},{income.Source},{transaction.Amount:F2},{runningBalance:F2}");
                    }
                    else if (transaction is BusinessExpense expense)
                    {
                        runningBalance -= transaction.Amount;
                        sb.AppendLine($"{transaction.TransactionID},{transaction.Date:yyyy-MM-dd},{expense.GetTransactionType()},{expense.Category},{transaction.Amount:F2},{runningBalance:F2}");
                    }
                }

                // Write to file
                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error generating Excel report: {ex.Message}", ex);
            }
        }
    }
}
