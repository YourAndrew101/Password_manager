using System.Collections.Generic;
using System.Data.SqlClient;
using UsersLibrary;
using static UsersLibrary.UsersExceptions;

namespace ServicesLibrary
{
    public static class UsersService
    {
        public static bool IsExistsEmail(string email)
        {
            string request = "SELECT COUNT(*) FROM \"users\" WHERE  EMAIL = @EMail";

            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@EMail", email);

            bool result = (int)command.ExecuteScalar() != 0;
            connection.Close();

            return result;
        }

        public static void AddUser(User user)
        {
            if (IsExistsEmail(user.Email)) throw new DuplicateMailException();

            string request = "INSERT INTO \"users\" (Email, Password, Salt) VALUES (@EMail, @Password, @Salt)";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@EMail", user.Email);
            command.Parameters.AddWithValue("@Password", user.HashAuthPassword);
            command.Parameters.AddWithValue("@Salt", user.Salt);

            command.ExecuteNonQuery();

            connection.Close();

            CreateUserServicesTable(user);
        }
        private static void CreateUserServicesTable(User user)
        {
            string request = $"CREATE TABLE \"{user.HashEmail}\" (Id int, Name varchar(255), Login varchar(255), Password varchar(255), LastDataAdd DateTime)";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;

            connection.Open();
            SqlCommand command = new SqlCommand(request, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }


        public static bool CheckUserData(User user)
        {
            if (!CheckPassword(user.Email, user.HashAuthPassword)) throw new IncorrectPasswordException();

            return true;
        }
        private static bool CheckPassword(string email, string password)
        {
            if (!IsExistsEmail(email)) throw new NonExistenMailException();

            Dictionary<string, string> sqlData = GetDataByEmail(email);
            string passwordCheck = sqlData["Password"];

            return password == passwordCheck;
        }

        private static Dictionary<string, string> GetDataByEmail(string email)
        {
            Dictionary<string, string> sqlData = new Dictionary<string, string>();

            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            string request = "SELECT * FROM \"users\" WHERE EMAIL = @EMail";
            SqlCommand command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@EMail", email);

            SqlDataReader sqlDataReader = command.ExecuteReader();
            while (sqlDataReader.Read())
                for (int i = 0; i < sqlDataReader.FieldCount; i++)
                    sqlData.Add(sqlDataReader.GetName(i), sqlDataReader.GetValue(i).ToString());

            connection.Close();

            return sqlData;
        }

        public static User GetHashAndSaltFromDB(User user)
        {
            if (!IsExistsEmail(user.Email)) throw new NonExistenMailException();

            Dictionary<string, string> sqlData = GetDataByEmail(user.Email);
            user.SetSaltAndHashAuthPassword(sqlData["Salt"]);

            return user;
        }
    }
}
