using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary;
using UsersLibrary.Services;

namespace Data.DataProviders.Products
{
    public class SQLDataProvider
    {
        private string _tableName;

        public List<Service> Load()
        {
            throw new NotImplementedException();
        }

        public void Save(User user, Service service)
        {
            throw new NotImplementedException();
        }

        public SQLDataProvider(string tableName)
        {
            _tableName = tableName;
        }
    }
}
