using System;
using System.Drawing;
using System.Windows.Forms;
using OmniLedger.Logic;
using System.Runtime.InteropServices;

namespace OmniLedger
{
    public partial class Form1 : Form
    {
        private UserManager _userManager;
        private bool isLoginMode = true;

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        public Form1()
        {
            InitializeComponent();
            _userManager = new UserManager();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            UpdateUI();
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

        private void ActionButton_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Please enter both username and password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (isLoginMode)
            {
                if (_userManager.ValidateUser(username, password))
                {
                    LedgerManager ledger = new LedgerManager(username);
                    Form2 dashboardForm = new Form2(ledger, username, _userManager);
                    dashboardForm.Show();
                    this.Hide();
                }
                else
                {
                    MessageBox.Show("Invalid username or password.", "Login Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                if (_userManager.RegisterUser(username, password))
                {
                    MessageBox.Show("Registration successful! You can now log in.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ToggleMode();
                }
                else
                {
                    MessageBox.Show("Username already exists or invalid input.", "Registration Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ToggleLabel_Click(object sender, EventArgs e)
        {
            ToggleMode();
        }

        private void ToggleMode()
        {
            isLoginMode = !isLoginMode;
            UpdateUI();
        }

        private void UpdateUI()
        {
            lblTitle.Text = isLoginMode ? "Welcome Back" : "Create Account";
            btnAction.Text = isLoginMode ? "LOGIN" : "SIGN UP";
            lblToggle.Text = isLoginMode ? "Don't have an account? Sign Up" : "Already have an account? Login";
            txtPassword.Text = "";
        }
    }
}
