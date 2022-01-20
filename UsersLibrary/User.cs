using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;
using System.Runtime.Serialization;

namespace UsersLibrary
{
    public class User
    {
        readonly Random _random = new Random();
        public static string ConnectionString { get; set; }

        private string _email;
        public string Email
        {
            get => _email;
            set
            {
                if (IsExistsEmail(value)) throw new DuplicateMailException(value);
                _email = value;
            }
        }

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
            Email = email;
            Password = password;

            Add();
        }


        private static bool IsExistsEmail(string email)
        {
            string request = "SELECT COUNT(*) FROM \"users\" WHERE  EMAIL = @EMail";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@EMail", email);

                return (int)command.ExecuteScalar() != 0;
            }
        }

        private static Dictionary<string, string> GetDataByEmail(string email)
        {
            Dictionary<string, string> sqlData = new Dictionary<string, string>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string request = "SELECT * FROM \"users\" WHERE EMAIL = @EMail";
                SqlCommand command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@EMail", email);

                SqlDataReader sqlDataReader = command.ExecuteReader();
                while (sqlDataReader.Read())
                    for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        sqlData.Add(sqlDataReader.GetName(i), sqlDataReader.GetValue(i).ToString());
            }

            return sqlData;
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

        public static bool CheckPassword(string email, string password)
        {
            if (!IsExistsEmail(email)) throw new NonExistenMailException(email);

            Dictionary<string, string> sqlData = GetDataByEmail(email);
            string passwordCheck = sqlData["Password"];
            password += sqlData["Salt"];

            using (SHA256 sHA256 = SHA256.Create())
            {
                byte[] hash = sHA256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return string.Join("", hash.Select(c => c.ToString("x2"))) == passwordCheck;
            }
        }
    }

    public class DuplicateMailException : Exception
    {
        public DuplicateMailException() { }
        public DuplicateMailException(string email) : base(string.Format("User with email - {0}, already exists", email)) { }
    }
    public class NonExistenMailException : Exception
    {
        public NonExistenMailException() { }
        public NonExistenMailException(string email) : base(string.Format("User with email - {0}, does not exists", email)) { }
    }
}