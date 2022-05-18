using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary;
using UsersLibrary.Services;

namespace Data.DataProviders.Products
{
    public interface IDataProvider
    {
        void Save(User user, Service services);
        List<Service> Load();

    }
}
