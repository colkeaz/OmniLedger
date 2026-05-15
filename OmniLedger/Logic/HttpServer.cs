using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web.Script.Serialization;
using System.Collections.Generic;

namespace OmniLedger.Logic
{
    public class HttpServer
    {
        private HttpListener _listener;
        private Thread _listenerThread;
        private bool _isRunning;
        private UserManager _userManager;
        private JavaScriptSerializer _serializer;

        public HttpServer(string url)
        {
            _listener = new HttpListener();
            _listener.Prefixes.Add(url);
            _userManager = new UserManager();
            _serializer = new JavaScriptSerializer();
        }

        public void Start(string url)
        {
            _listener.Start();
            _isRunning = true;
            _listenerThread = new Thread(Listen);
            _listenerThread.Start();
            Console.WriteLine($"Server started on {url}");
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
        }

        private void Listen()
        {
            while (_isRunning)
            {
                try
                {
                    var context = _listener.GetContext();
                    ProcessRequest(context);
                }
                catch (HttpListenerException)
                {
                    // Ignore exceptions during shutdown
                }
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            var request = context.Request;
            var response = context.Response;

            // Enable CORS
            response.AppendHeader("Access-Control-Allow-Origin", "*");
            response.AppendHeader("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
            response.AppendHeader("Access-Control-Allow-Headers", "Content-Type");

            if (request.HttpMethod == "OPTIONS")
            {
                response.StatusCode = 200;
                response.Close();
                return;
            }

            try
            {
                string path = request.Url.LocalPath.ToLower();
                if (request.HttpMethod == "POST" && path == "/api/auth/login")
                {
                    HandleLogin(request, response);
                }
                else if (request.HttpMethod == "POST" && path == "/api/auth/register")
                {
                    HandleRegister(request, response);
                }
                else if (request.HttpMethod == "GET" && path == "/api/ledger/dashboard")
                {
                    HandleDashboard(request, response);
                }
                else if (request.HttpMethod == "POST" && path == "/api/ledger/transaction")
                {
                    HandleTransaction(request, response);
                }
                else if (request.HttpMethod == "POST" && path == "/api/ledger/currency")
                {
                    HandleCurrency(request, response);
                }
                else if (request.HttpMethod == "GET" && path == "/api/ledger/export")
                {
                    HandleExport(request, response);
                }
                else if (request.HttpMethod == "GET" && path == "/api/ledger/export-pdf")
                {
                    HandleExportPdf(request, response);
                }
                else
                {
                    SendJsonResponse(response, 404, new { error = "Not Found" });
                }
            }
            catch (Exception ex)
            {
                SendJsonResponse(response, 500, new { error = ex.Message });
            }
        }

        private void HandleLogin(HttpListenerRequest req, HttpListenerResponse res)
        {
            var data = ReadJsonBody<Dictionary<string, string>>(req);
            if (data != null && data.ContainsKey("username") && data.ContainsKey("password"))
            {
                bool success = _userManager.ValidateUser(data["username"], data["password"]);
                if (success)
                    SendJsonResponse(res, 200, new { success = true, username = data["username"] });
                else
                    SendJsonResponse(res, 401, new { success = false, message = "Invalid credentials" });
            }
            else
            {
                SendJsonResponse(res, 400, new { success = false, message = "Invalid request" });
            }
        }

        private void HandleRegister(HttpListenerRequest req, HttpListenerResponse res)
        {
            var data = ReadJsonBody<Dictionary<string, string>>(req);
            if (data != null && data.ContainsKey("username") && data.ContainsKey("password"))
            {
                bool success = _userManager.RegisterUser(data["username"], data["password"]);
                if (success)
                    SendJsonResponse(res, 200, new { success = true });
                else
                    SendJsonResponse(res, 400, new { success = false, message = "Username already exists" });
            }
            else
            {
                SendJsonResponse(res, 400, new { success = false, message = "Invalid request" });
            }
        }

        private void HandleDashboard(HttpListenerRequest req, HttpListenerResponse res)
        {
            string username = req.QueryString["username"];
            if (string.IsNullOrEmpty(username))
            {
                SendJsonResponse(res, 400, new { error = "Username required" });
                return;
            }

            var user = _userManager.GetUser(username);
            string defaultCurrency = user != null ? user.PreferredCurrency : "$";

            var ledger = new LedgerManager(username, defaultCurrency);
            var transactions = ledger.GetAllTransactions();
            
            // Format transactions for the frontend
            var formattedTx = new List<object>();
            foreach (var t in transactions)
            {
                formattedTx.Add(new {
                    id = t.TransactionID,
                    date = t.Date.ToString("O"),
                    source = t.Description,
                    amount = t.Amount,
                    isPositive = t is IncomeRecord
                });
            }

            var data = new {
                balance = ledger.CurrentBalance,
                currency = ledger.CurrentCurrencySymbol,
                totalIncome = ledger.GetTotalIncome(),
                totalExpenses = ledger.GetTotalExpenses(),
                history = formattedTx
            };

            SendJsonResponse(res, 200, data);
        }

        private void HandleTransaction(HttpListenerRequest req, HttpListenerResponse res)
        {
            var data = ReadJsonBody<Dictionary<string, object>>(req);
            if (data != null && data.ContainsKey("username") && data.ContainsKey("amount") && data.ContainsKey("description") && data.ContainsKey("type"))
            {
                string username = data["username"].ToString();
                decimal amount;
                if (!decimal.TryParse(data["amount"].ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out amount))
                {
                    SendJsonResponse(res, 400, new { error = "Invalid amount" });
                    return;
                }
                string description = data["description"].ToString();
                string type = data["type"].ToString();

                var user = _userManager.GetUser(username);
                string userCurrency = user != null ? user.PreferredCurrency : "$";
                string inputCurrency = data.ContainsKey("currency") ? data["currency"].ToString() : userCurrency;

                if (inputCurrency != userCurrency)
                {
                    amount = CurrencyConverter.Convert(amount, inputCurrency, userCurrency);
                }

                var ledger = new LedgerManager(username, userCurrency);
                Transaction t = type == "Income" ? (Transaction)new IncomeRecord(amount, "", description) : new BusinessExpense(amount, "", description);
                bool success = ledger.ProcessTransaction(t);
                
                if (success)
                    SendJsonResponse(res, 200, new { success = true });
                else
                    SendJsonResponse(res, 400, new { success = false, message = "Transaction failed (possibly insufficient funds)" });
            }
            else
            {
                SendJsonResponse(res, 400, new { success = false, message = "Invalid request payload" });
            }
        }

        private void HandleCurrency(HttpListenerRequest req, HttpListenerResponse res)
        {
            var data = ReadJsonBody<Dictionary<string, string>>(req);
            if (data != null && data.ContainsKey("username") && data.ContainsKey("currency"))
            {
                string username = data["username"];
                string newCurrency = data["currency"];

                var user = _userManager.GetUser(username);
                if (user == null)
                {
                    SendJsonResponse(res, 404, new { success = false, message = "User not found" });
                    return;
                }

                string oldCurrency = user.PreferredCurrency;

                // Load with old currency, then change it, which saves to CSV
                var ledger = new LedgerManager(username, oldCurrency);
                ledger.ChangeCurrency(newCurrency);

                // Update default currency in UserManager memory/file
                _userManager.UpdateUserCurrency(username, newCurrency);

                SendJsonResponse(res, 200, new { success = true });
            }
            else
            {
                SendJsonResponse(res, 400, new { success = false, message = "Invalid request payload" });
            }
        }

        private void HandleExport(HttpListenerRequest req, HttpListenerResponse res)
        {
            string username = req.QueryString["username"];
            if (string.IsNullOrEmpty(username))
            {
                SendJsonResponse(res, 400, new { error = "Username required" });
                return;
            }

            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"{username}_ledger.csv");
            if (!File.Exists(filePath))
            {
                SendJsonResponse(res, 404, new { error = "Ledger not found" });
                return;
            }

            res.StatusCode = 200;
            res.ContentType = "text/csv";
            res.AddHeader("Content-Disposition", $"attachment; filename=\"{username}_ledger.csv\"");
            
            byte[] fileBytes = File.ReadAllBytes(filePath);
            res.ContentLength64 = fileBytes.Length;
            using (var output = res.OutputStream)
            {
                output.Write(fileBytes, 0, fileBytes.Length);
            }
            res.Close();
        }

        private void HandleExportPdf(HttpListenerRequest req, HttpListenerResponse res)
        {
            string username = req.QueryString["username"];
            if (string.IsNullOrEmpty(username))
            {
                SendJsonResponse(res, 400, new { error = "Username required" });
                return;
            }

            var user = _userManager.GetUser(username);
            string defaultCurrency = user != null ? user.PreferredCurrency : "$";

            var ledger = new LedgerManager(username, defaultCurrency);
            var transactions = ledger.GetAllTransactions();

            // Generate PDF report to a temp file
            string tempPath = Path.Combine(Path.GetTempPath(), $"{username}_report.pdf");
            try
            {
                IReportGenerator pdfExporter = new PdfExporter();
                pdfExporter.GenerateReport(transactions, ledger.CurrentBalance, tempPath);

                res.StatusCode = 200;
                res.ContentType = "application/pdf";
                res.AddHeader("Content-Disposition", $"attachment; filename=\"{username}_report.pdf\"");

                byte[] fileBytes = File.ReadAllBytes(tempPath);
                res.ContentLength64 = fileBytes.Length;
                using (var output = res.OutputStream)
                {
                    output.Write(fileBytes, 0, fileBytes.Length);
                }
                res.Close();
            }
            finally
            {
                // Clean up temp file
                if (File.Exists(tempPath))
                    File.Delete(tempPath);
            }
        }

        private T ReadJsonBody<T>(HttpListenerRequest request)
        {
            var encoding = request.ContentEncoding ?? Encoding.UTF8;
            using (var reader = new StreamReader(request.InputStream, encoding))
            {
                string json = reader.ReadToEnd();
                return _serializer.Deserialize<T>(json);
            }
        }

        private void SendJsonResponse(HttpListenerResponse response, int statusCode, object data)
        {
            response.StatusCode = statusCode;
            response.ContentType = "application/json; charset=utf-8";
            string json = _serializer.Serialize(data);
            byte[] bytes = Encoding.UTF8.GetBytes(json);
            response.ContentLength64 = bytes.Length;
            using (var output = response.OutputStream)
            {
                output.Write(bytes, 0, bytes.Length);
            }
            response.Close();
        }
    }
}
