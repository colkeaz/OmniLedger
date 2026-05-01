using System;

namespace OmniLedger.Logic
{
    public class User
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string PreferredCurrency { get; set; }
        
        public User() 
        { 
            PreferredCurrency = "$";
        }
        
        public User(string username, string passwordHash, string preferredCurrency = "$")
        {
            Username = username;
            PasswordHash = passwordHash;
            PreferredCurrency = preferredCurrency;
        }
    }
}
