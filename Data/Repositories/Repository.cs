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

        public bool Any() => Context.Services.Any();


        public Service Get(int id) => Context.Services.First(s => s.Id == id);
        public List<Service> GetAll() => Context.Services;


        public void Add(Service service)
        {
            service.Id = Context.Services.Any() ? Context.Services.Max(s => s.Id) + 1 : 1;
            Context.Services.Add(service);
        }


        public void Remove(Service service) => Context.Services.Remove(service);
        public void RemoveRange(IEnumerable<Service> services)
        {
            foreach (Service item in services)
                Context.Services.Remove(item);
        }


        public void Update(int id, Service entity)
        {
            Service tempEntity = Context.Services.First(c => c.Id == id);
            if (tempEntity != null)
            {
                tempEntity.Name  = entity.Name;
                tempEntity.Login = entity.Login;
                tempEntity.Password = entity.Password;
            }
        }
        public void Update(Service startEntity, Service finishEntity) => Update(startEntity.Id, finishEntity);
    }
}
