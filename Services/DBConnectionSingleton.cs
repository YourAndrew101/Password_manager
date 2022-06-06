using System.Data.SqlClient;

namespace ServicesLibrary
{
    public class DBConnectionSingleton
    {
        private DBConnectionSingleton() { }

        private static DBConnectionSingleton _instance;
        private static readonly object _lock = new object();

        public SqlConnection SqlConnection { get; private set; }

        public static string ConnectionString { private get; set; }

        public static DBConnectionSingleton GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DBConnectionSingleton();
                        _instance.SqlConnection = new SqlConnection(ConnectionString);
                    }
                }
            }

            return _instance;
        }
    }
}
