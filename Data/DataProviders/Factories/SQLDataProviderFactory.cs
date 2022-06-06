using Data.DataProviders.Products;

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
