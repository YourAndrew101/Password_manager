using System.Collections.Generic;
using UsersLibrary.Services;

namespace Data
{
    public class DataContext
    {
        internal List<Service> Services { get; private set; }

        public DataContext() => Services = new List<Service>(); 
        public DataContext(List<Service> services) => Services = services; 
    }
}
