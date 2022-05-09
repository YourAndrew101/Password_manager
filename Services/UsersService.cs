using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary;
using static UsersLibrary.UsersExceptions;

namespace ServicesLibrary
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

                bool result = (int)command.ExecuteScalar() != 0;
                connection.Close();

                return result;
            }
        }      
        
        public static void IsUserExists(string email)
        {
            if (!IsExistsEmail(email)) throw new NonExistenMailException();
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
                command.Parameters.AddWithValue("@Password", user.HashAuthPassword);
                command.Parameters.AddWithValue("@Salt", user.Salt);

                command.ExecuteNonQuery();

                connection.Close();
            }

            CreateUserServicesTable(user);
        }
        private static void CreateUserServicesTable(User user)
        {
            string request = $"CREATE TABLE \"{user.HashEmail}\" (Id int, Name varchar(255), Login varchar(255), Password varchar(255))";
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {           
                connection.Open();
                SqlCommand command = new SqlCommand(request, connection);
                command.ExecuteNonQuery();

                connection.Close();
            }
        }


        public static bool CheckUserData(string email, string password)
        {
            if (!CheckPassword(email, password)) throw new IncorrectPasswordException();

            return true;
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

                connection.Close();
            }

            return sqlData;
        }

        public static User GetHashAndSaltFromDB(User user)
        {
            if (!IsExistsEmail(user.Email)) throw new NonExistenMailException();

            Dictionary<string, string> sqlData = GetDataByEmail(user.Email);
            user.SetHashAuthPassword(sqlData["Salt"]);

            return user;
        }

        private static bool CheckPassword(string email, string password)
        {
            if (!IsExistsEmail(email)) throw new NonExistenMailException();

            Dictionary<string, string> sqlData = GetDataByEmail(email);
            string passwordCheck = sqlData["Password"];
            password += sqlData["Salt"];

            return User.GetSHA256Hash(password) == passwordCheck;
        }
    }
}
