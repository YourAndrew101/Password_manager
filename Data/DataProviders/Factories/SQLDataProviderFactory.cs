using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DataProviders.Products;
using ServicesLibrary;
using UsersLibrary;

namespace Data.DataProviders.Factories
{
    public class SQLDataProviderFactory : CommonDataProviderFactory
    {
        public override IDataProvider GetDataProvider()
        {
            return new SQLDataProvider();
        }
    }
}
