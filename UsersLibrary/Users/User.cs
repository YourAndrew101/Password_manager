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
using UsersLibrary.Settings;

namespace UsersLibrary
{
    public class User
    {
        readonly Random _random = new Random();


        public string _email;
        public string Email
        {
            get => _email;
            internal set
            {
                _email = value;
                HashEmail = GetSHA256Hash(value);
            }
        }
        public string HashEmail { get; private set; }


        private string _password;
        public string AuthPassword { get; private set; }
        public string HashAuthPassword
        {
            get => _password;
            private set
            {
                value += Salt;
                _password = GetSHA256Hash(value);
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

            private set => _salt = value;
        }


        public User() { }
        public User(string email, string password)
        {
            Email = email;
            AuthPassword = password;
            HashAuthPassword = password;
        }

        public User(string email, string password, string salt)
        {
            Email = email;
            AuthPassword = password;
            HashAuthPassword = password;
            Salt = salt; 
        }


        private string GetSHA256Hash(string inputString)
        {
            string outString;
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                outString = string.Join("", hash.Select(c => c.ToString("x2")));
            }

            return outString;
        }

        public static implicit operator User(Settings.Settings settings)
        {
            return new User(settings.Email, settings.Password, settings.Salt);
        }
    }
}