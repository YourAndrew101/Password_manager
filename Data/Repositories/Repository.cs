using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using UsersLibrary.Services;

namespace Data.Repositories
{
    public class ServiceRepository : IRepository<Service>
    {
        protected DataContext Context { get; private set; }

        public ServiceRepository(DataContext dataContext)
        {
            Context = dataContext;
        }

        public Service Get(int id)
        {
            return Context.Services.First(s => s.Id == id);
        }

        public IEnumerable<Service> GetAll()
        {
            return Context.Services;
        }

        public IEnumerable<Service> Find(Func<Service, bool> predicate)
        {
            return Context.Services.Where(predicate);
        }

        public void Add(Service service)
        {
            service.Id = Context.Services.Any() ? Context.Services.Max(s => s.Id) : 0;
            Context.Services.Add(service);
        }

        public void AddRange(IEnumerable<Service> services)
        {
            Context.Services.AddRange(services);
        }

        public void Remove(Service service)
        {
            Context.Services.Remove(service);
        }

        public void RemoveRange(IEnumerable<Service> services)
        {
            foreach (Service item in services)
                Context.Services.Remove(item);
        }
    }
}
