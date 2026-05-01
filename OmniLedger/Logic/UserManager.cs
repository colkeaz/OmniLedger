using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace OmniLedger.Logic
{
    public class UserManager
    {
        private string _userFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "users.txt");
        private List<User> _users;

        public UserManager()
        {
            _users = new List<User>();
            LoadUsers();
        }

        private void LoadUsers()
        {
            if (!File.Exists(_userFilePath)) return;

            var lines = File.ReadAllLines(_userFilePath);
            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length >= 2)
                {
                    string currency = parts.Length > 2 ? parts[2] : "$";
                    _users.Add(new User(parts[0], parts[1], currency));
                }
            }
        }

        private void SaveUsers()
        {
            var lines = _users.Select(u => $"{u.Username},{u.PasswordHash},{u.PreferredCurrency}");
            File.WriteAllLines(_userFilePath, lines);
        }

        public bool RegisterUser(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password)) return false;
            if (_users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase))) return false;

            _users.Add(new User(username, HashPassword(password)));
            SaveUsers();
            return true;
        }

        public bool ValidateUser(string username, string password)
        {
            var user = GetUser(username);
            if (user == null) return false;

            return user.PasswordHash == HashPassword(password);
        }

        public User GetUser(string username)
        {
            return _users.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public void UpdateUserCurrency(string username, string newCurrency)
        {
            var user = GetUser(username);
            if (user != null)
            {
                user.PreferredCurrency = newCurrency;
                SaveUsers();
            }
        }

        private string HashPassword(string password)
        {
            using (var sha256 = System.Security.Cryptography.SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
