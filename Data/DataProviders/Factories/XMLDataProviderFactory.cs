using Data.DataProviders.Products;

namespace Data.DataProviders.Factories
{
    public class XMLDataProviderFactory : CommonDataProviderFactory
    {
        public override IDataProvider GetDataProvider()
        {
            return new XMLDataProvider();
        }
    }
}
