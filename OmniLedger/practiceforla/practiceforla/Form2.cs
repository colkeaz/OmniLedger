using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace practiceforla
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void DrawSimpleChart(Graphics g)
        {
            // 1. Get your data (Replace these with your actual logic/variables)
            decimal income = 5000;
            decimal expense = 3500;
            decimal maxAmount = Math.Max(income, expense) * 1.2m; // Leave 20% "headroom" at the top

            // 2. Clear the background
            g.Clear(BarChart.BackColor);

            // 3. Calculate scaling (Pixels per Dollar)
            // Height of panel / max amount = how many pixels represent $1
            float scale = (float)BarChart.Height / (float)maxAmount;

            // 4. Calculate Bar Heights
            int incHeight = (int)(income * (decimal)scale);
            int expHeight = (int)(expense * (decimal)scale);

            // 5. Draw the Bars (X, Y, Width, Height)
            // Note: Y must be (PanelHeight - BarHeight) because 0 is the TOP
            g.FillRectangle(Brushes.MediumSeaGreen, 40, BarChart.Height - incHeight, 50, incHeight);
            g.FillRectangle(Brushes.IndianRed, 110, BarChart.Height - expHeight, 50, expHeight);

            // 6. Optional: Add small labels
            g.DrawString("Inc", this.Font, Brushes.Black, 50, BarChart.Height - 20);
            g.DrawString("Exp", this.Font, Brushes.Black, 120, BarChart.Height - 20);
        }

        private void BarChart_Paint(object sender, PaintEventArgs e)
        {
            DrawSimpleChart(e.Graphics);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click_1(object sender, EventArgs e)
        {

        }
    }
}
