using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DataProviders.Products;
using ServicesLibrary;
using UsersLibrary;

namespace Data.DataProviders.Factories
{
    public abstract class IDataProviderFactory
    {
        public abstract IDataProvider GetDataProvider();

        public IDataProviderFactory[] CreateFactory()
        {
            if (InternetService.IsConnectedToInternet)
                return new IDataProviderFactory[] { new SQLDataProviderFactory(), new XMLDataProviderFactory() };

            return new IDataProviderFactory[] { new XMLDataProviderFactory()};
        }
    }
}
