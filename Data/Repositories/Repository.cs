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

        public void Add(Service entity)
        {
            Context.Services.Add(entity);


        }

        public void AddRange(IEnumerable<Service> entities)
        {
            Context.Services.AddRange(entities);
        }

        public void Remove(Service entity)
        {
            Context.Services.Remove(entity);
        }

        public void RemoveRange(IEnumerable<Service> entities)
        {
            foreach (var item in entities)
                Context.Services.Remove(item);
        }
    }
}
