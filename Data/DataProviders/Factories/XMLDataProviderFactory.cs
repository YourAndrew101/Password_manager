using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DataProviders.Products;

namespace Data.DataProviders.Factories
{
    public class XMLDataProviderFactory : IDataProviderFactory
    {
        public override IDataProvider GetDataProvider()
        {
            return new XMLDataProvider();
        }
    }
}
