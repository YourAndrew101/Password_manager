using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary;
using UsersLibrary.Services;

namespace Data.DataProviders.Products
{
    public class BinaryDataProvider : IDataProvider
    {
        private string _fileName;

        public List<Service> Load()
        {
            throw new NotImplementedException();
        }

        public void Save(User user, Service services)
        {
            throw new NotImplementedException();
        }

        public void Delete(User user, Service service)
        {
            throw new NotImplementedException();
        }

        public List<Service> Load(User user)
        {
            throw new NotImplementedException();
        }

        public void Clear(User user)
        {
            throw new NotImplementedException();
        }

        public BinaryDataProvider(string fileName)
        {
            _fileName = fileName;
        }
    }
}
