namespace OmniLedger
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.btnMinimize = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.pnlUserLine = new System.Windows.Forms.Panel();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.pnlPassLine = new System.Windows.Forms.Panel();
            this.btnAction = new System.Windows.Forms.Button();
            this.lblToggle = new System.Windows.Forms.Label();
            this.lblUserIcon = new System.Windows.Forms.Label();
            this.lblPassIcon = new System.Windows.Forms.Label();
            this.pnlHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(45)))), ((int)(((byte)(45)))), ((int)(((byte)(48)))));
            this.pnlHeader.Controls.Add(this.lblLogo);
            this.pnlHeader.Controls.Add(this.btnMinimize);
            this.pnlHeader.Controls.Add(this.btnClose);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(400, 35);
            this.pnlHeader.TabIndex = 0;
            this.pnlHeader.MouseDown += new System.Windows.Forms.MouseEventHandler(this.HeaderPanel_MouseDown);
            // 
            // lblLogo
            // 
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = System.Drawing.Color.White;
            this.lblLogo.Location = new System.Drawing.Point(12, 9);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(86, 17);
            this.lblLogo.TabIndex = 2;
            this.lblLogo.Text = "OmniLedger";
            // 
            // btnMinimize
            // 
            this.btnMinimize.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnMinimize.FlatAppearance.BorderSize = 0;
            this.btnMinimize.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinimize.ForeColor = System.Drawing.Color.White;
            this.btnMinimize.Location = new System.Drawing.Point(330, 0);
            this.btnMinimize.Name = "btnMinimize";
            this.btnMinimize.Size = new System.Drawing.Size(35, 35);
            this.btnMinimize.TabIndex = 1;
            this.btnMinimize.Text = "—";
            this.btnMinimize.UseVisualStyleBackColor = true;
            this.btnMinimize.Click += new System.EventHandler(this.MinimizeButton_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Red;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.Location = new System.Drawing.Point(365, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(35, 35);
            this.btnClose.TabIndex = 0;
            this.btnClose.Text = "X";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.CloseButton_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 80);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 30);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Welcome Back";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblUserIcon
            // 
            this.lblUserIcon.AutoSize = true;
            this.lblUserIcon.ForeColor = System.Drawing.Color.Gray;
            this.lblUserIcon.Location = new System.Drawing.Point(50, 160);
            this.lblUserIcon.Name = "lblUserIcon";
            this.lblUserIcon.Size = new System.Drawing.Size(20, 15);
            this.lblUserIcon.TabIndex = 6;
            this.lblUserIcon.Text = "👤";
            // 
            // txtUsername
            // 
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtUsername.ForeColor = System.Drawing.Color.White;
            this.txtUsername.Location = new System.Drawing.Point(80, 160);
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(250, 20);
            this.txtUsername.TabIndex = 2;
            // 
            // pnlUserLine
            // 
            this.pnlUserLine.BackColor = System.Drawing.Color.Gray;
            this.pnlUserLine.Location = new System.Drawing.Point(50, 185);
            this.pnlUserLine.Name = "pnlUserLine";
            this.pnlUserLine.Size = new System.Drawing.Size(280, 1);
            this.pnlUserLine.TabIndex = 3;
            // 
            // lblPassIcon
            // 
            this.lblPassIcon.AutoSize = true;
            this.lblPassIcon.ForeColor = System.Drawing.Color.Gray;
            this.lblPassIcon.Location = new System.Drawing.Point(50, 230);
            this.lblPassIcon.Name = "lblPassIcon";
            this.lblPassIcon.Size = new System.Drawing.Size(20, 15);
            this.lblPassIcon.TabIndex = 7;
            this.lblPassIcon.Text = "🔒";
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.txtPassword.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPassword.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPassword.ForeColor = System.Drawing.Color.White;
            this.txtPassword.Location = new System.Drawing.Point(80, 230);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(250, 20);
            this.txtPassword.TabIndex = 4;
            this.txtPassword.UseSystemPasswordChar = true;
            // 
            // pnlPassLine
            // 
            this.pnlPassLine.BackColor = System.Drawing.Color.Gray;
            this.pnlPassLine.Location = new System.Drawing.Point(50, 255);
            this.pnlPassLine.Name = "pnlPassLine";
            this.pnlPassLine.Size = new System.Drawing.Size(280, 1);
            this.pnlPassLine.TabIndex = 5;
            // 
            // btnAction
            // 
            this.btnAction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.btnAction.FlatAppearance.BorderSize = 0;
            this.btnAction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAction.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAction.ForeColor = System.Drawing.Color.White;
            this.btnAction.Location = new System.Drawing.Point(50, 310);
            this.btnAction.Name = "btnAction";
            this.btnAction.Size = new System.Drawing.Size(280, 40);
            this.btnAction.TabIndex = 8;
            this.btnAction.Text = "LOGIN";
            this.btnAction.UseVisualStyleBackColor = false;
            this.btnAction.Click += new System.EventHandler(this.ActionButton_Click);
            // 
            // lblToggle
            // 
            this.lblToggle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblToggle.ForeColor = System.Drawing.Color.DarkGray;
            this.lblToggle.Location = new System.Drawing.Point(0, 370);
            this.lblToggle.Name = "lblToggle";
            this.lblToggle.Size = new System.Drawing.Size(400, 20);
            this.lblToggle.TabIndex = 9;
            this.lblToggle.Text = "Don\'t have an account? Sign Up";
            this.lblToggle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblToggle.Click += new System.EventHandler(this.ToggleLabel_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.lblToggle);
            this.Controls.Add(this.btnAction);
            this.Controls.Add(this.pnlPassLine);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblPassIcon);
            this.Controls.Add(this.pnlUserLine);
            this.Controls.Add(this.txtUsername);
            this.Controls.Add(this.lblUserIcon);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlHeader);
            this.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OmniLedger Login";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnMinimize;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Panel pnlUserLine;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Panel pnlPassLine;
        private System.Windows.Forms.Button btnAction;
        private System.Windows.Forms.Label lblToggle;
        private System.Windows.Forms.Label lblUserIcon;
        private System.Windows.Forms.Label lblPassIcon;
    }
}
