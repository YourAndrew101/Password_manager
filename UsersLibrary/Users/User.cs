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
using UsersLibrary.Services;

namespace UsersLibrary
{
    public class User
    {
        readonly Random _random = new Random();

        private string _email;
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


        public List<Service> Services { get; private set; }


        private string _hashPassword;
        public string AuthPassword { get; private set; }
        public string HashAuthPassword
        {
            get => _hashPassword;
            private set
            {
                value += Salt;
                _hashPassword = GetSHA256Hash(value);
            }
        }

        public string CryptoPassword { get; set; }

        public string Salt { get; set; }
        private void CreateSalt()
        {
            if (string.IsNullOrEmpty(Salt))
                Salt = string.Join("", Enumerable.Range(0, 20).Select(_ => (char)_random.Next(65, 90)));
        }


        public User() { }
        public User(string email, string password)
        {
            CreateSalt();

            Email = email;
            AuthPassword = password;
            HashAuthPassword = password;

            Services = new List<Service>();
        }


        public static User CreateAlreadyExistUser(string email, string password)
        {
            return new User()
            {
                Email = email,
                AuthPassword = password,
            };
        }
        public void SetHashAuthPassword(string salt)
        {
            Salt = salt;
            HashAuthPassword = AuthPassword;
        }

        public static string GetSHA256Hash(string inputString)
        {
            string outString;
            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(inputString));
                outString = string.Join("", hash.Select(c => c.ToString("x2")));
            }

            return outString;
        }

        public static implicit operator User(Settings.SignUpSettings settings)
        {
            return new User()
            {
                Email = settings.Email,
                AuthPassword = settings.Password
            };

        }
    }
}