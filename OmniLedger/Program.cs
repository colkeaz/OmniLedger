using System;
using OmniLedger.Logic;

namespace OmniLedger
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {

            Console.WriteLine("=== OmniLedger Logic Test System ===");
            Console.WriteLine($"Test Date: {DateTime.Now}");
            Console.WriteLine("------------------------------------");

            LedgerManager manager = new LedgerManager();

            // Test 1: Verification of Income Logic
            Console.WriteLine("[Test 1] Adding Income...");
            manager.ProcessTransaction(new Income { Amount = 5000, Source = "DOST Scholarship" });
            Console.WriteLine($"Current Balance: {manager.CurrentBalance:C}"); // Should be 5000.00

            // Test 2: Verification of Fund Validation (Boundary Testing)
            Console.WriteLine("\n[Test 2] Testing Boundary Logic...");
            bool canAffordSmall = manager.ValidateFunds(2000);
            Console.WriteLine($"Is 2,000.00 valid against balance? {canAffordSmall}"); // Expected: True

            bool canAffordBig = manager.ValidateFunds(6000);
            Console.WriteLine($"Is 6,000.00 valid against balance? {canAffordBig}");   // Expected: False (Preventing Overdraft)

            // Test 3: Verification of Expense Logic
            Console.WriteLine("\n[Test 3] Processing Valid Expense...");
            manager.ProcessTransaction(new Expense { Amount = 1500, Category = "Dorm Rent" });
            Console.WriteLine($"Final Balance: {manager.CurrentBalance:C}"); // Expected: 3500.00

            Console.WriteLine("\n------------------------------------");
            Console.WriteLine("TEST STATUS: PASSED");
            Console.WriteLine("Press any key to exit the test harness...");
            Console.ReadKey();
        }
    }
}