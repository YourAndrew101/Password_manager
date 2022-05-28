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
        void Save(User user, Service service);
        void Save(User user, IEnumerable<Service> services);

        void Delete(User user, Service service);
        void Delete(User user, int id);

        void Update(User user, int id, Service service);
        void Update(User user, Service startService, Service finishService);

        List<Service> Load(User user);
        void Clear(User user);

        DateTime GetLastModifyTime(User user);
    }
}
