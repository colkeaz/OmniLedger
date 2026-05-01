using System;
using System.Drawing;
using System.Windows.Forms;
using OmniLedger.Logic;
using System.Runtime.InteropServices;

namespace OmniLedger
{
    public partial class Form2 : Form
    {
        private LedgerManager _ledgerManager;
        private string _username;
        private UserManager _userManager;
        private string _currencySymbol;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form2(LedgerManager ledgerManager, string username, UserManager userManager)
        {
            InitializeComponent();
            _ledgerManager = ledgerManager;
            _username = username;
            _userManager = userManager;
            
            var user = _userManager.GetUser(_username);
            _currencySymbol = user != null ? user.PreferredCurrency : "$";
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            lblUserDisplay.Text = $"Welcome, {_username}";
            StyleDataGridView();
            RefreshDashboard();
        }

        private void HeaderPanel_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void MinimizeButton_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void StyleDataGridView()
        {
            dataGridView1.BorderStyle = BorderStyle.None;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 40, 40);
            dataGridView1.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.RowsDefaultCellStyle.BackColor = Color.FromArgb(30, 30, 30);
            dataGridView1.RowsDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(0, 122, 204);
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView1.BackgroundColor = Color.FromArgb(30, 30, 30);
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(45, 45, 48);
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.RowHeadersVisible = false;
        }

        private void RefreshDashboard()
        {
            UpdateBalance();
            UpdateTransactionGrid();
            BarChart.Invalidate();
        }

        private string FormatCurrency(decimal amount) => $"{_currencySymbol}{amount:0.00}";

        private void UpdateBalance()
        {
            TotalBalance.Text = FormatCurrency(_ledgerManager.CurrentBalance);
        }

        private void UpdateTransactionGrid()
        {
            dataGridView1.Rows.Clear();
            var transactions = _ledgerManager.GetAllTransactions();
            
            foreach (var transaction in transactions)
            {
                dataGridView1.Rows.Add(
                    transaction.TransactionID,
                    transaction.Date.ToString("yyyy-MM-dd"),
                    transaction.GetTransactionType(),
                    GetTransactionDescription(transaction),
                    FormatCurrency(transaction.Amount)
                );
            }
        }

        private string GetTransactionDescription(Transaction transaction)
        {
            if (transaction is IncomeRecord income) return income.Source;
            else if (transaction is BusinessExpense expense) return expense.Category;
            return transaction.Description;
        }

        private void button1_Click(object sender, EventArgs e) // Add Income
        {
            decimal rawAmount = PromptForAmountAndCurrency("Enter Income Amount:", out string transCurrency);
            if (rawAmount > 0)
            {
                string source = PromptForString("Enter Income Source:");
                if (!string.IsNullOrEmpty(source))
                {
                    decimal convertedAmount = CurrencyConverter.Convert(rawAmount, transCurrency, _currencySymbol);
                    var incomeRecord = new IncomeRecord(convertedAmount, source, "");
                    _ledgerManager.ProcessTransaction(incomeRecord);
                    RefreshDashboard();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e) // Add Expense
        {
            decimal rawAmount = PromptForAmountAndCurrency("Enter Expense Amount:", out string transCurrency);
            if (rawAmount > 0)
            {
                string category = PromptForString("Enter Expense Category:");
                if (!string.IsNullOrEmpty(category))
                {
                    decimal convertedAmount = CurrencyConverter.Convert(rawAmount, transCurrency, _currencySymbol);
                    var expense = new BusinessExpense(convertedAmount, category, "");
                    if (_ledgerManager.ProcessTransaction(expense))
                    {
                        RefreshDashboard();
                    }
                    else
                    {
                        MessageBox.Show("Insufficient funds for this expense!", "Transaction Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private decimal PromptForAmountAndCurrency(string prompt, out string selectedCurrency)
        {
            selectedCurrency = "$";
            Form promptForm = new Form()
            {
                Text = "Input",
                Width = 300,
                Height = 200,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            Label label = new Label() { Left = 20, Top = 20, Text = prompt, Width = 260 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            
            ComboBox comboBox = new ComboBox() 
            { 
                Left = 20, 
                Top = 90, 
                Width = 240, 
                BackColor = Color.FromArgb(30, 30, 30), 
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };

            comboBox.Items.Add("$ - USD");
            comboBox.Items.Add("€ - EUR");
            comboBox.Items.Add("£ - GBP");
            comboBox.Items.Add("¥ - JPY");
            comboBox.Items.Add("₱ - PHP");
            comboBox.Items.Add("₹ - INR");
            
            int index = comboBox.FindString(_currencySymbol);
            comboBox.SelectedIndex = index >= 0 ? index : 0;

            Button okButton = new Button() { Text = "OK", Left = 130, Width = 80, Top = 140, DialogResult = DialogResult.OK, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204) };
            okButton.FlatAppearance.BorderSize = 0;
            Button cancelButton = new Button() { Text = "Cancel", Left = 210, Width = 80, Top = 140, DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat, BackColor = Color.Gray };
            cancelButton.FlatAppearance.BorderSize = 0;

            promptForm.Controls.Add(label);
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(comboBox);
            promptForm.Controls.Add(okButton);
            promptForm.Controls.Add(cancelButton);
            promptForm.AcceptButton = okButton;
            promptForm.CancelButton = cancelButton;

            while (true)
            {
                if (promptForm.ShowDialog() == DialogResult.OK)
                {
                    if (decimal.TryParse(textBox.Text, out decimal amount) && amount > 0)
                    {
                        selectedCurrency = comboBox.SelectedItem.ToString().Split(' ')[0];
                        return amount;
                    }
                    MessageBox.Show("Please enter a valid positive amount.", "Invalid Input", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    return -1;
                }
            }
        }

        private string PromptForString(string prompt)
        {
            return PromptDialog(prompt);
        }

        private string PromptDialog(string prompt)
        {
            Form promptForm = new Form()
            {
                Text = "Input",
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            Label label = new Label() { Left = 20, Top = 20, Text = prompt, Width = 260 };
            TextBox textBox = new TextBox() { Left = 20, Top = 50, Width = 240, BackColor = Color.FromArgb(30, 30, 30), ForeColor = Color.White, BorderStyle = BorderStyle.FixedSingle };
            Button okButton = new Button() { Text = "OK", Left = 130, Width = 80, Top = 90, DialogResult = DialogResult.OK, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204) };
            okButton.FlatAppearance.BorderSize = 0;
            Button cancelButton = new Button() { Text = "Cancel", Left = 210, Width = 80, Top = 90, DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat, BackColor = Color.Gray };
            cancelButton.FlatAppearance.BorderSize = 0;

            promptForm.Controls.Add(label);
            promptForm.Controls.Add(textBox);
            promptForm.Controls.Add(okButton);
            promptForm.Controls.Add(cancelButton);
            promptForm.AcceptButton = okButton;
            promptForm.CancelButton = cancelButton;

            return promptForm.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }

        private void button4_Click(object sender, EventArgs e) // Export
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "CSV Files (*.csv)|*.csv|PDF Files (*.pdf)|*.pdf|Text Files (*.txt)|*.txt";
                saveFileDialog.Title = "Export Report";
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        IReportGenerator exporter;
                        if (saveFileDialog.FileName.EndsWith(".csv")) exporter = new ExcelExporter();
                        else if (saveFileDialog.FileName.EndsWith(".pdf")) exporter = new PdfExporter();
                        else exporter = new ExcelExporter();

                        exporter.GenerateReport(_ledgerManager.GetAllTransactions(), _ledgerManager.CurrentBalance, saveFileDialog.FileName);
                        MessageBox.Show($"Report exported successfully to {saveFileDialog.FileName}", "Export Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error exporting report: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void button8_Click_1(object sender, EventArgs e) // Logout
        {
            this.Hide();
            Form1 loginForm = new Form1();
            loginForm.Show();
        }

        private void btnCurrency_Click(object sender, EventArgs e) // Change Currency
        {
            string newCurrency = PromptForCurrency();
            if (!string.IsNullOrEmpty(newCurrency))
            {
                _currencySymbol = newCurrency.Trim();
                _userManager.UpdateUserCurrency(_username, _currencySymbol);
                RefreshDashboard();
            }
        }

        private string PromptForCurrency()
        {
            Form promptForm = new Form()
            {
                Text = "Select Currency",
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.None,
                StartPosition = FormStartPosition.CenterParent,
                BackColor = Color.FromArgb(45, 45, 48),
                ForeColor = Color.White
            };

            Label label = new Label() { Left = 20, Top = 20, Text = "Select your preferred currency:", Width = 260 };
            
            ComboBox comboBox = new ComboBox() 
            { 
                Left = 20, 
                Top = 50, 
                Width = 240, 
                BackColor = Color.FromArgb(30, 30, 30), 
                ForeColor = Color.White,
                DropDownStyle = ComboBoxStyle.DropDownList,
                FlatStyle = FlatStyle.Flat
            };

            comboBox.Items.Add("$ - USD");
            comboBox.Items.Add("€ - EUR");
            comboBox.Items.Add("£ - GBP");
            comboBox.Items.Add("¥ - JPY");
            comboBox.Items.Add("₱ - PHP");
            comboBox.Items.Add("₹ - INR");
            comboBox.SelectedIndex = 0;

            Button okButton = new Button() { Text = "OK", Left = 130, Width = 80, Top = 90, DialogResult = DialogResult.OK, FlatStyle = FlatStyle.Flat, BackColor = Color.FromArgb(0, 122, 204) };
            okButton.FlatAppearance.BorderSize = 0;
            Button cancelButton = new Button() { Text = "Cancel", Left = 210, Width = 80, Top = 90, DialogResult = DialogResult.Cancel, FlatStyle = FlatStyle.Flat, BackColor = Color.Gray };
            cancelButton.FlatAppearance.BorderSize = 0;

            promptForm.Controls.Add(label);
            promptForm.Controls.Add(comboBox);
            promptForm.Controls.Add(okButton);
            promptForm.Controls.Add(cancelButton);
            promptForm.AcceptButton = okButton;
            promptForm.CancelButton = cancelButton;

            if (promptForm.ShowDialog() == DialogResult.OK)
            {
                string selection = comboBox.SelectedItem.ToString();
                return selection.Split(' ')[0];
            }
            return "";
        }

        private void DrawSimpleChart(Graphics g)
        {
            decimal income = _ledgerManager.GetTotalIncome();
            decimal expense = _ledgerManager.GetTotalExpenses();
            decimal maxAmount = Math.Max(income, expense) * 1.2m;

            g.Clear(Color.FromArgb(30, 30, 30));
            if (maxAmount == 0) maxAmount = 1;

            float scale = (float)BarChart.Height / (float)maxAmount;
            int incHeight = (int)((float)income * scale);
            int expHeight = (int)((float)expense * scale);

            Brush incBrush = new SolidBrush(Color.FromArgb(46, 204, 113));
            Brush expBrush = new SolidBrush(Color.FromArgb(231, 76, 60));

            g.FillRectangle(incBrush, 40, BarChart.Height - incHeight, 80, incHeight);
            g.FillRectangle(expBrush, 140, BarChart.Height - expHeight, 80, expHeight);

            g.DrawString("Income", this.Font, Brushes.White, 45, BarChart.Height - 25);
            g.DrawString("Expense", this.Font, Brushes.White, 135, BarChart.Height - 25);
            
            g.DrawString(FormatCurrency(income), this.Font, Brushes.White, 40, BarChart.Height - incHeight - 25);
            g.DrawString(FormatCurrency(expense), this.Font, Brushes.White, 140, BarChart.Height - expHeight - 25);

            incBrush.Dispose();
            expBrush.Dispose();
        }

        private void BarChart_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            DrawSimpleChart(e.Graphics);
        }
    }
}
