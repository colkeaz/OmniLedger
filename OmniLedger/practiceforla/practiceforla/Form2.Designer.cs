namespace practiceforla
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            button1 = new Button();
            button2 = new Button();
            dataGridView1 = new DataGridView();
            Column1 = new DataGridViewTextBoxColumn();
            Column2 = new DataGridViewTextBoxColumn();
            Column3 = new DataGridViewComboBoxColumn();
            Column4 = new DataGridViewTextBoxColumn();
            Column5 = new DataGridViewComboBoxColumn();
            label1 = new Label();
            button4 = new Button();
            panel2 = new Panel();
            button8 = new Button();
            button5 = new Button();
            button6 = new Button();
            label3 = new Label();
            TotalBalance = new Label();
            BarChart = new Panel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // button1
            // 
            button1.BackColor = Color.FromArgb(192, 255, 192);
            button1.Location = new Point(42, 649);
            button1.Name = "button1";
            button1.Size = new Size(198, 54);
            button1.TabIndex = 2;
            button1.Text = "+ Income";
            button1.UseVisualStyleBackColor = false;
            button1.Click += button1_Click;
            // 
            // button2
            // 
            button2.BackColor = Color.FromArgb(255, 192, 192);
            button2.Location = new Point(255, 649);
            button2.Name = "button2";
            button2.Size = new Size(199, 54);
            button2.TabIndex = 3;
            button2.Text = "- Expense";
            button2.UseVisualStyleBackColor = false;
            button2.Click += button2_Click;
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToResizeColumns = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.BackgroundColor = SystemColors.Control;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column5 });
            dataGridView1.Location = new Point(42, 150);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 82;
            dataGridView1.Size = new Size(864, 465);
            dataGridView1.TabIndex = 4;
            dataGridView1.CellContentClick += dataGridView1_CellContentClick;
            // 
            // Column1
            // 
            Column1.HeaderText = "ID";
            Column1.MinimumWidth = 10;
            Column1.Name = "Column1";
            Column1.Width = 80;
            // 
            // Column2
            // 
            Column2.HeaderText = "Date";
            Column2.MinimumWidth = 10;
            Column2.Name = "Column2";
            Column2.Resizable = DataGridViewTriState.True;
            Column2.Width = 150;
            // 
            // Column3
            // 
            Column3.HeaderText = "Description";
            Column3.MinimumWidth = 10;
            Column3.Name = "Column3";
            Column3.Width = 200;
            // 
            // Column4
            // 
            Column4.HeaderText = "Amount";
            Column4.MinimumWidth = 10;
            Column4.Name = "Column4";
            Column4.Width = 200;
            // 
            // Column5
            // 
            Column5.HeaderText = "Type";
            Column5.MinimumWidth = 10;
            Column5.Name = "Column5";
            Column5.Width = 150;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 28.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(928, 150);
            label1.Name = "label1";
            label1.Size = new Size(719, 90);
            label1.TabIndex = 6;
            label1.Text = "Monthly Cash Flow";
            // 
            // button4
            // 
            button4.BackColor = Color.FromArgb(252, 251, 244);
            button4.FlatStyle = FlatStyle.Flat;
            button4.Location = new Point(1437, 649);
            button4.Name = "button4";
            button4.Size = new Size(217, 54);
            button4.TabIndex = 7;
            button4.Text = "Export";
            button4.UseVisualStyleBackColor = false;
            button4.Click += button4_Click;
            // 
            // panel2
            // 
            panel2.Controls.Add(button8);
            panel2.Controls.Add(button5);
            panel2.Dock = DockStyle.Top;
            panel2.Location = new Point(0, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(1698, 46);
            panel2.TabIndex = 8;
            // 
            // button8
            // 
            button8.FlatStyle = FlatStyle.Popup;
            button8.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button8.Location = new Point(1531, 0);
            button8.Name = "button8";
            button8.Size = new Size(167, 46);
            button8.TabIndex = 3;
            button8.Text = "Logout";
            button8.UseVisualStyleBackColor = true;
            button8.Click += button8_Click_1;
            // 
            // button5
            // 
            button5.FlatStyle = FlatStyle.Popup;
            button5.Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            button5.Location = new Point(0, 0);
            button5.Name = "button5";
            button5.Size = new Size(167, 46);
            button5.TabIndex = 0;
            button5.Text = "Dashboard";
            button5.UseVisualStyleBackColor = true;
            button5.Click += button5_Click;
            // 
            // button6
            // 
            button6.FlatStyle = FlatStyle.Popup;
            button6.Location = new Point(706, 649);
            button6.Name = "button6";
            button6.Size = new Size(199, 54);
            button6.TabIndex = 9;
            button6.Text = "Refresh";
            button6.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.BackColor = Color.FromArgb(200, 208, 208);
            label3.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(44, 96);
            label3.Name = "label3";
            label3.Size = new Size(237, 39);
            label3.TabIndex = 10;
            label3.Text = "Total Balance:";
            label3.Click += label3_Click;
            // 
            // TotalBalance
            // 
            TotalBalance.AutoSize = true;
            TotalBalance.BackColor = Color.FromArgb(200, 208, 208);
            TotalBalance.Font = new Font("Century Gothic", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            TotalBalance.Location = new Point(273, 96);
            TotalBalance.Name = "TotalBalance";
            TotalBalance.Size = new Size(80, 39);
            TotalBalance.TabIndex = 11;
            TotalBalance.Text = "0.00";
            // 
            // BarChart
            // 
            BarChart.Location = new Point(928, 244);
            BarChart.Name = "BarChart";
            BarChart.Size = new Size(726, 371);
            BarChart.TabIndex = 12;
            BarChart.Paint += BarChart_Paint;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(252, 251, 244);
            ClientSize = new Size(1698, 749);
            Controls.Add(BarChart);
            Controls.Add(TotalBalance);
            Controls.Add(label3);
            Controls.Add(button6);
            Controls.Add(panel2);
            Controls.Add(button4);
            Controls.Add(label1);
            Controls.Add(dataGridView1);
            Controls.Add(button2);
            Controls.Add(button1);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "OmniLedger";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            panel2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button button1;
        private Button button2;
        private DataGridView dataGridView1;
        private DataGridViewTextBoxColumn Column1;
        private DataGridViewTextBoxColumn Column2;
        private DataGridViewComboBoxColumn Column3;
        private DataGridViewTextBoxColumn Column4;
        private DataGridViewComboBoxColumn Column5;
        private Label label1;
        private Button button4;
        private Panel panel2;
        private Button button5;
        private Button button8;
        private Button button6;
        private Label label3;
        private Label TotalBalance;
        private Panel BarChart;
    }
}