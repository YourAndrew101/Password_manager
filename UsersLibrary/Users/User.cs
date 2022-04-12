using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;
using static UsersLibrary.UsersExceptions;

namespace UsersLibrary
{
    public class User
    {
        readonly Random _random = new Random();

        public string Email { get; set; }

        private string _password;
        public string AuthPassword
        {
            get => _password;
            set
            {
                value += Salt;
                using (SHA256 sHA256 = SHA256.Create())
                {
                    byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(value));
                    _password = string.Join("", hash.Select(c => c.ToString("x2")));
                }
            }
        }

        public string CryptoPassword { get; set; }

        private string _salt;
        public string Salt
        {
            get
            {
                if (string.IsNullOrEmpty(_salt)) _salt = string.Join("", Enumerable.Range(0, 20).Select(_ => (char)_random.Next(65, 90)));
                return _salt;
            }
        }

        public User() { }
        public User(string email, string password)
        {
            Email = email;
            AuthPassword = password;
            CryptoPassword = password;
        }    
    }
}