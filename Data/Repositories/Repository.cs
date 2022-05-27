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

        public bool Any()
        {
            return Context.Services.Any();
        }

        public Service Get(int id)
        {
            return Context.Services.First(s => s.Id == id);
        }

        public ICollection<Service> GetAll()
        {
            return Context.Services;
        }

        public void Add(Service service)
        {
            service.Id = Context.Services.Any() ? Context.Services.Max(s => s.Id) : 1;
            Context.Services.Add(service);
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

        public void Update(int id, Service entity)
        {
            throw new NotImplementedException();
        }

        public void Update(Service startEntity, Service finishEntity)
        {
            throw new NotImplementedException();
        }
    }
}
