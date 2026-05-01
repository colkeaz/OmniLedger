using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OmniLedger.Logic
{
    /// <summary>
    /// PdfExporter - Implementation of IReportGenerator
    /// Exports financial data to PDF format
    /// Note: This is a simplified implementation. In production, use a PDF library like iTextSharp or PdfSharp
    /// </summary>
    public class PdfExporter : IReportGenerator
    {
        public string GetFileExtension() => ".pdf";

        public void GenerateReport(List<Transaction> transactions, decimal totalBalance, string filePath)
        {
            try
            {
                // Simplified PDF generation (creates a text-based representation)
                // In production, use iTextSharp or similar library
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("%PDF-1.4");
                sb.AppendLine("1 0 obj");
                sb.AppendLine("<< >>");
                sb.AppendLine("endobj");
                sb.AppendLine();

                // Simple text representation
                sb.AppendLine("=== OmniLedger Financial Report (PDF) ===");
                sb.AppendLine($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                sb.AppendLine();
                sb.AppendLine($"Total Balance: {totalBalance:C}");
                sb.AppendLine();
                sb.AppendLine("--- Transaction Details ---");

                decimal runningBalance = 0;

                foreach (var transaction in transactions)
                {
                    if (transaction is IncomeRecord income)
                    {
                        runningBalance += transaction.Amount;
                        sb.AppendLine($"[{transaction.TransactionID}] {transaction.Date:yyyy-MM-dd} - {income.Source}: +{transaction.Amount:C}");
                    }
                    else if (transaction is BusinessExpense expense)
                    {
                        runningBalance -= transaction.Amount;
                        sb.AppendLine($"[{transaction.TransactionID}] {transaction.Date:yyyy-MM-dd} - {expense.Category}: -{transaction.Amount:C}");
                    }
                }

                sb.AppendLine();
                sb.AppendLine($"Final Balance: {totalBalance:C}");
                sb.AppendLine("=== End of Report ===");

                File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error generating PDF report: {ex.Message}", ex);
            }
        }
    }
}
