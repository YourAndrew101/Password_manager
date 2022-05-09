using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Services
{
    public class ServiceContext : DbContext
    {
        public static string ConnectionString { get; set; }

        public ServiceContext(string connectionString) : base(connectionString)
        {
            ConnectionString = connectionString;
        }

        public DbSet<Service> Services { get; set; }
    }
}
