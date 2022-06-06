using Data.DataProviders.Products;
using ServicesLibrary;

namespace Data.DataProviders.Factories
{
    public class CommonDataProviderFactory
    {
        public virtual IDataProvider GetDataProvider() { return null; }

        public static CommonDataProviderFactory[] CreateFactories()
        {
            if (InternetService.IsConnectedToInternet)
                return new CommonDataProviderFactory[] { new SQLDataProviderFactory(), new XMLDataProviderFactory() };

            return new CommonDataProviderFactory[] { new XMLDataProviderFactory()};
        }
    }
}
