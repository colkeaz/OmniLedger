using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using BCrypt.Net;

namespace OmniLedger.Logic
{
    public class UserManager
    {
        private string _userFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");
        private List<User> _users;
        private readonly object _syncRoot = new object();

        public UserManager()
        {
            _users = new List<User>();
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (!File.Exists(_userFilePath)) return;

            var lines = File.ReadAllLines(_userFilePath, Encoding.UTF8);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string currency = parts.Length > 2 ? CurrencyConverter.SanitizeCurrency(parts[2]) : "$";
                    _users.Add(new User(parts[0], parts[1], currency));
                }
            }
        }

        private void SaveUsers()
        {
            var lines = _users.Select(u => $"{u.Username},{u.PasswordHash},{u.PreferredCurrency}");
            File.WriteAllLines(_userFilePath, lines, Encoding.UTF8);
        }

        public bool RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;

            // Hash the password outside the lock. BCrypt is intentionally slow, 
            // so we avoid blocking other threads during the calculation.
            string hashedPassword = HashPassword(password);

            lock (_syncRoot)
            {
                if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;

                _users.Add(new User(username, hashedPassword));
                SaveUsers();
                return true;
            }
        }

        public bool ValidateUser(string username, string password)
        {
            User user;
            
            // Retrieve the user inside the lock
            lock (_syncRoot)
            {
                user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }

            if (user == null) return false;

            // Verify outside the lock. This extracts the salt from the stored hash 
            // and securely compares it against the plaintext input.
            return BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        }

        public User GetUser(string username)
        {
            lock (_syncRoot)
            {
                return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
        }

        public void UpdateUserCurrency(string username, string newCurrency)
        {
            // Note: Added a lock here to prevent race conditions during the save operation
            lock (_syncRoot)
            {
                var user = _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    user.PreferredCurrency = newCurrency;
                    SaveUsers();
                }
            }
        }

        private string HashPassword(string password)
        {
            // Generates a salted hash with a default work factor (cost) of 11.
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
