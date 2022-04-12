using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary;
using static UsersLibrary.UsersExceptions;

namespace Services
{
    public static class UsersService
    {
        public static string ConnectionString { get; set; }

        public static bool IsExistsEmail(string email)
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

        public static void AddUser(User user)
        {
            if (IsExistsEmail(user.Email)) throw new DuplicateMailException();

            string request = "INSERT INTO \"users\" (Email, Password, Salt) VALUES (@EMail, @Password, @Salt)";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@EMail", user.Email);
                command.Parameters.AddWithValue("@Password", user.AuthPassword);
                command.Parameters.AddWithValue("@Salt", user.Salt);

                command.ExecuteNonQuery();
            }
        }

        public static User GetUser(string email, string password)
        {
            if (!CheckPassword(email, password)) throw new IncorrectPasswordException();

            return new User(email, password);
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

        private static bool CheckPassword(string email, string password)
        {
            if (!IsExistsEmail(email)) throw new NonExistenMailException();

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
}
