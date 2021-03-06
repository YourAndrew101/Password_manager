using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using ServicesLibrary;
using UsersLibrary;
using UsersLibrary.Services;

namespace Data.DataProviders.Products
{
    public class SQLDataProvider : IDataProvider
    {
        public List<Service> Load(User user)
        {
            List<Service> list = new List<Service>();

            string request = $"Select * from \"{user.HashEmail}\"";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);

            using (SqlDataReader reader = command.ExecuteReader())
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        int id = Convert.ToInt32(reader["ID"]);
                        string name = Convert.ToString(reader["Name"]);
                        string login = Convert.ToString(reader["Login"]);
                        string password = Convert.ToString(reader["Password"]);

                        CryptedService service = new CryptedService(name, login, password) { Id = id };
                        list.Add(user.DecryptService(service));
                    }
                    reader.Close();
                }
            }

            connection.Close();
            return list;
        }

        public void Save(User user, Service service)
        {
            CryptedService cryptedService = user.EncryptService(service);

            string request = $"INSERT INTO \"{user.HashEmail}\" (Id, Name, Login, Password, LastDataAdd) VALUES (@Id, @Name, @Login, @Password, @LastDataAdd)";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@Id", cryptedService.Id);
            command.Parameters.AddWithValue("@Name", cryptedService.Name);
            command.Parameters.AddWithValue("@Login", cryptedService.Login);
            command.Parameters.AddWithValue("@Password", cryptedService.Password);
            command.Parameters.AddWithValue("@LastDataAdd", DateTime.Now);
            command.ExecuteNonQuery();

            connection.Close();
        }
        public void Save(User user, IEnumerable<Service> services)
        {
            string request = $"INSERT INTO \"{user.HashEmail}\" (Id, Name, Login, Password, LastDataAdd) VALUES (@Id, @Name, @Login, @Password, @LastDataAdd)";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            foreach (Service service in services)
            {
                CryptedService cryptedService = user.EncryptService(service);

                SqlCommand command = new SqlCommand(request, connection);
                command.Parameters.AddWithValue("@Id", cryptedService.Id);
                command.Parameters.AddWithValue("@Name", cryptedService.Name);
                command.Parameters.AddWithValue("@Login", cryptedService.Login);
                command.Parameters.AddWithValue("@Password", cryptedService.Password);
                command.Parameters.AddWithValue("@LastDataAdd", DateTime.Now);
                command.ExecuteNonQuery();
            }

            connection.Close();
        }

        public void Delete(User user, Service service)
        {
            string request = $"DELETE FROM \"{user.HashEmail}\" WHERE Id = {service.Id}";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
        public void Delete(User user, int id)
        {
            string request = $"DELETE FROM \"{user.HashEmail}\" WHERE Id = {id}";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public void Update(User user, int id, Service service)
        {
            CryptedService cryptedService = user.EncryptService(service);

            string request = $"UPDATE \"{user.HashEmail}\" SET Id = @Id, Name = @Name, Login = @Login, Password = @Password, LastDataAdd = @LastDataAdd WHERE Id = {id}";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.Parameters.AddWithValue("@Id", cryptedService.Id);
            command.Parameters.AddWithValue("@Name", cryptedService.Name);
            command.Parameters.AddWithValue("@Login", cryptedService.Login);
            command.Parameters.AddWithValue("@Password", cryptedService.Password);
            command.Parameters.AddWithValue("@LastDataAdd", DateTime.Now);
            command.ExecuteNonQuery();

            connection.Close();
        }
        public void Update(User user, Service startService, Service finishService) => Update(user, startService.Id, finishService);

        public void Clear(User user)
        {
            string request = $"DELETE FROM \"{user.HashEmail}\"";
            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            SqlCommand command = new SqlCommand(request, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }

        public DateTime GetLastModifyTime(User user)
        {
            if (!UsersService.IsExistsEmail(user.Email)) return DateTime.MinValue;

            SqlConnection connection = DBConnectionSingleton.GetInstance().SqlConnection;
            connection.Open();

            string request = $"select LastDataAdd from \"{user.HashEmail}\" order by LastDataAdd desc";
            SqlCommand command = new SqlCommand(request, connection);
            SqlDataReader sqlDataReader = command.ExecuteReader();

            DateTime dateTime = new DateTime();
            if (sqlDataReader.HasRows)
            {
                sqlDataReader.Read();
                dateTime = (DateTime)sqlDataReader["LastDataAdd"];
            }

            sqlDataReader.Close();
            connection.Close();
            return dateTime;
        }
    }
}
