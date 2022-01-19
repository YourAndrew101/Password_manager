using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

namespace UsersLibrary
{
    public class User
    {
        readonly Random _random = new Random();
        public static string ConnectionString { get; set; }

        public string Email { get; set; }

        private string _password;
        public string Password
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
            if (IsExists(email)) throw new Exception($"User with email - {email} already exists.");

            Email = email;
            Password = password;

            Add();
        }

        private static bool IsExists(string email)
        {
            using (SqlConnection myConnection = new SqlConnection(ConnectionString))
            {
                string request = $"SELECT * FROM \"users\" WHERE EMAIL = @EMail";
                SqlCommand command = new SqlCommand(request, myConnection);
                command.Parameters.AddWithValue("@EMail", email);

                myConnection.Open();

                using (SqlDataReader oReader = command.ExecuteReader())
                    return oReader.HasRows;
            }
        }
        private void Add()
        {
            string request = "INSERT INTO \"users\" (Email, Password, Salt) VALUES (@EMail, @Password, @Salt)";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@EMail", Email);
                command.Parameters.AddWithValue("@Password", Password);
                command.Parameters.AddWithValue("@Salt", Salt);

                command.ExecuteNonQuery();
            }
        }
    }
}