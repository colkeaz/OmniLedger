using System;
using System.Windows.Forms;
using OmniLedger.Logic;
using OmniLedger.UI;

namespace OmniLedger
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}