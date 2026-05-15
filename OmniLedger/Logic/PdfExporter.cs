using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace OmniLedger.Logic
{
    /// <summary>
    /// PdfExporter - Implementation of IReportGenerator
    /// Exports financial data to a valid PDF format using raw PDF specification.
    /// No external libraries required — generates a standards-compliant PDF 1.4 document.
    /// </summary>
    public class PdfExporter : IReportGenerator
    {
        public string GetFileExtension() => ".pdf";

        public void GenerateReport(List<Transaction> transactions, decimal totalBalance, string filePath)
        {
            try
            {
                // Collect all text lines for the report
                var lines = new List<string>();
                lines.Add("OmniLedger Financial Report");
                lines.Add("");
                lines.Add($"Generated: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
                lines.Add($"Total Balance: {totalBalance:F2}");
                lines.Add("");
                lines.Add("--- Transaction Details ---");
                lines.Add("");
                lines.Add(String.Format("{0,-6} {1,-12} {2,-10} {3,-20} {4,12}", "ID", "Date", "Type", "Description", "Amount"));
                lines.Add(new string('-', 64));

                foreach (var transaction in transactions)
                {
                    if (transaction is IncomeRecord income)
                    {
                        lines.Add(String.Format("{0,-6} {1,-12} {2,-10} {3,-20} {4,12}",
                            transaction.TransactionID,
                            transaction.Date.ToString("yyyy-MM-dd"),
                            "Income",
                            TruncateString(income.Source.Length > 0 ? income.Source : transaction.Description, 18),
                            $"+{transaction.Amount:F2}"));
                    }
                    else if (transaction is BusinessExpense expense)
                    {
                        lines.Add(String.Format("{0,-6} {1,-12} {2,-10} {3,-20} {4,12}",
                            transaction.TransactionID,
                            transaction.Date.ToString("yyyy-MM-dd"),
                            "Expense",
                            TruncateString(expense.Category.Length > 0 ? expense.Category : transaction.Description, 18),
                            $"-{transaction.Amount:F2}"));
                    }
                }

                lines.Add("");
                lines.Add(new string('-', 64));
                lines.Add($"Final Balance: {totalBalance:F2}");
                lines.Add("");
                lines.Add("=== End of Report ===");

                // Build valid PDF binary
                byte[] pdfBytes = BuildPdf(lines);
                File.WriteAllBytes(filePath, pdfBytes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Error generating PDF report: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Builds a valid PDF 1.4 binary from a list of text lines.
        /// Uses the raw PDF specification to create a standards-compliant document
        /// with a Courier font for monospaced table alignment.
        /// </summary>
        private byte[] BuildPdf(List<string> lines)
        {
            // PDF uses fixed byte offsets, so we track positions carefully
            using (var ms = new MemoryStream())
            {
                var offsets = new List<long>(); // byte offset of each object

                // --- Header ---
                WriteAscii(ms, "%PDF-1.4\n");
                // Binary comment to mark this as binary PDF (recommended by spec)
                ms.Write(new byte[] { 0x25, 0xE2, 0xE3, 0xCF, 0xD3, 0x0A }, 0, 6);

                // --- Object 1: Catalog ---
                offsets.Add(ms.Position);
                WriteAscii(ms, "1 0 obj\n<< /Type /Catalog /Pages 2 0 R >>\nendobj\n");

                // --- Object 2: Pages ---
                offsets.Add(ms.Position);
                WriteAscii(ms, "2 0 obj\n<< /Type /Pages /Kids [3 0 R] /Count 1 >>\nendobj\n");

                // --- Object 4: Font ---
                offsets.Add(ms.Position);
                WriteAscii(ms, "4 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Courier >>\nendobj\n");

                // --- Object 5: Bold Font for title ---
                offsets.Add(ms.Position);
                WriteAscii(ms, "5 0 obj\n<< /Type /Font /Subtype /Type1 /BaseFont /Courier-Bold >>\nendobj\n");

                // --- Build the page content stream ---
                // PDF Td operator is RELATIVE, so we use it once for initial position,
                // then TL (text leading) + T* (next line) for all subsequent lines.
                var contentSb = new StringBuilder();

                float pageHeight = 842f; // A4 height in points
                float marginTop = 60f;
                float marginLeft = 50f;
                float bodyLineHeight = 13f;
                float titleLineHeight = 22f;
                float startY = pageHeight - marginTop;

                // --- Title block (bold, larger font) ---
                if (lines.Count > 0)
                {
                    contentSb.Append("BT\n");
                    contentSb.AppendFormat("/F2 16 Tf\n");
                    contentSb.AppendFormat("{0} {1} Td\n", FormatFloat(marginLeft), FormatFloat(startY));
                    contentSb.AppendFormat("({0}) Tj\n", EscapePdfString(lines[0]));
                    contentSb.Append("ET\n");
                    startY -= titleLineHeight;
                }

                // --- Body block (normal font, all remaining lines) ---
                if (lines.Count > 1)
                {
                    contentSb.Append("BT\n");
                    contentSb.AppendFormat("/F1 9 Tf\n");
                    contentSb.AppendFormat("{0} TL\n", FormatFloat(bodyLineHeight));
                    contentSb.AppendFormat("{0} {1} Td\n", FormatFloat(marginLeft), FormatFloat(startY));

                    for (int i = 1; i < lines.Count; i++)
                    {
                        if (startY - ((i - 1) * bodyLineHeight) < 40) break; // stop before going off-page

                        if (i == 1)
                        {
                            // First body line: already positioned by Td above
                            contentSb.AppendFormat("({0}) Tj\n", EscapePdfString(lines[i]));
                        }
                        else
                        {
                            // Subsequent lines: T* moves down by TL amount, then Tj shows text
                            contentSb.AppendFormat("T* ({0}) Tj\n", EscapePdfString(lines[i]));
                        }
                    }

                    contentSb.Append("ET\n");
                }
                string contentStream = contentSb.ToString();
                byte[] contentBytes = Encoding.ASCII.GetBytes(contentStream);

                // --- Object 6: Content Stream ---
                offsets.Add(ms.Position);
                WriteAscii(ms, $"6 0 obj\n<< /Length {contentBytes.Length} >>\nstream\n");
                ms.Write(contentBytes, 0, contentBytes.Length);
                WriteAscii(ms, "\nendstream\nendobj\n");

                // --- Object 3: Page ---
                offsets.Add(ms.Position);
                WriteAscii(ms, "3 0 obj\n<< /Type /Page /Parent 2 0 R /MediaBox [0 0 595 842] " +
                    "/Contents 6 0 R /Resources << /Font << /F1 4 0 R /F2 5 0 R >> >> >>\nendobj\n");

                // --- Cross-reference table ---
                long xrefStart = ms.Position;

                // Map object numbers to their offsets
                // Objects: 1(catalog), 2(pages), 4(font), 5(bold font), 6(stream), 3(page)
                int[] objNumbers = { 1, 2, 4, 5, 6, 3 };
                int maxObj = 6;

                // Build a lookup: objNumber -> offset
                var objOffsetMap = new Dictionary<int, long>();
                for (int i = 0; i < objNumbers.Length; i++)
                {
                    objOffsetMap[objNumbers[i]] = offsets[i];
                }

                WriteAscii(ms, "xref\n");
                WriteAscii(ms, $"0 {maxObj + 1}\n");
                WriteAscii(ms, "0000000000 65535 f \n"); // object 0 (free)
                for (int i = 1; i <= maxObj; i++)
                {
                    long offset = objOffsetMap.ContainsKey(i) ? objOffsetMap[i] : 0;
                    WriteAscii(ms, $"{offset:D10} 00000 n \n");
                }

                // --- Trailer ---
                WriteAscii(ms, $"trailer\n<< /Size {maxObj + 1} /Root 1 0 R >>\n");
                WriteAscii(ms, "startxref\n");
                WriteAscii(ms, $"{xrefStart}\n");
                WriteAscii(ms, "%%EOF\n");

                return ms.ToArray();
            }
        }

        /// <summary>
        /// Escapes special PDF string characters: backslash, parens
        /// </summary>
        private string EscapePdfString(string input)
        {
            if (string.IsNullOrEmpty(input)) return "";
            return input
                .Replace("\\", "\\\\")
                .Replace("(", "\\(")
                .Replace(")", "\\)");
        }

        /// <summary>
        /// Formats a float for PDF coordinates (invariant culture, no trailing zeros)
        /// </summary>
        private string FormatFloat(float value)
        {
            return value.ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Writes an ASCII string to the stream
        /// </summary>
        private void WriteAscii(MemoryStream ms, string text)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(text);
            ms.Write(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Truncates a string to a max length, adding "..." if needed
        /// </summary>
        private string TruncateString(string input, int maxLength)
        {
            if (string.IsNullOrEmpty(input)) return "";
            if (input.Length <= maxLength) return input;
            return input.Substring(0, maxLength - 2) + "..";
        }
    }
}
