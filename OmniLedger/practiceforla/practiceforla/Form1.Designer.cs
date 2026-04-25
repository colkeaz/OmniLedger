namespace practiceforla
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Century Gothic", 28.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.Location = new Point(88, 68);
            label1.Name = "label1";
            label1.Size = new Size(491, 90);
            label1.TabIndex = 0;
            label1.Text = "OmniLedger";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.InactiveBorder;
            textBox1.Location = new Point(260, 255);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(310, 37);
            textBox1.TabIndex = 1;
            textBox1.TextChanged += textBox1_TextChanged;
            // 
            // textBox2
            // 
            textBox2.BackColor = SystemColors.InactiveBorder;
            textBox2.Location = new Point(260, 316);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(310, 37);
            textBox2.TabIndex = 2;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Century Gothic", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.Location = new Point(92, 256);
            label2.Name = "label2";
            label2.Size = new Size(153, 33);
            label2.TabIndex = 3;
            label2.Text = "Username:";
            label2.Click += label2_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Century Gothic", 10.125F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label3.Location = new Point(101, 317);
            label3.Name = "label3";
            label3.Size = new Size(145, 33);
            label3.TabIndex = 4;
            label3.Text = "Password:";
            label3.Click += label3_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Century Gothic", 8F);
            label4.Location = new Point(393, 158);
            label4.Name = "label4";
            label4.Size = new Size(177, 25);
            label4.TabIndex = 5;
            label4.Text = "Finance Ledger";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(252, 251, 244);
            ClientSize = new Size(680, 437);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label1);
            Font = new Font("Century Gothic", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            Name = "Form1";
            Text = "OmniLedger";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBox1;
        private TextBox textBox2;
        private Label label2;
        private Label label3;
        private Label label4;
    }
}
