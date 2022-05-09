using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UsersLibrary.Services
{ 
    public class ServiceContext : DbContext
    {
        public string _tableName;
        public static string ConnectionString { get; set; }

        public ServiceContext(string connectionString) : base(connectionString)
        {       
            ConnectionString = connectionString;
        }

        public DbSet<Service> Services { get; set; }
    }
}
