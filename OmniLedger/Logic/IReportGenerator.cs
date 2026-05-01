using System;
using System.Collections.Generic;

namespace OmniLedger.Logic
{
    /// <summary>
    /// Abstraction: Interface for report generation
    /// Allows multiple implementations (Excel, PDF) without changing core logic
    /// </summary>
    public interface IReportGenerator
    {
        /// <summary>
        /// Generates a report from transaction data
        /// </summary>
        void GenerateReport(List<Transaction> transactions, decimal totalBalance, string filePath);

        /// <summary>
        /// Returns the file extension for this report type
        /// </summary>
        string GetFileExtension();
    }
}
