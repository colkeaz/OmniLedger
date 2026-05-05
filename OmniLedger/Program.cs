using System;
using OmniLedger.Logic;

namespace OmniLedger
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string url = "http://localhost:8080/";
            var server = new HttpServer(url);
            server.Start(url);

            Console.WriteLine("API Server is running. Press Enter to exit...");
            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

            server.Stop();
        }
    }
}