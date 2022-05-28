using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Data.DataProviders.Factories;
using UsersLibrary;
using UsersLibrary.Services;

namespace Data.Repositories
{
    public class ServiceRepository : IRepository<Service>
    {
        protected DataContext Context { get; private set; }
        private User User { get; set; }

        public ServiceRepository(DataContext dataContext, User user)
        {
            Context = dataContext;
            User = user;
        }

        public bool Any() => Context.Services.Any();


        public Service Get(int id) => Context.Services.First(s => s.Id == id);
        public List<Service> GetAll() => Context.Services;


        public void Add(Service service)
        {
            service.Id = Context.Services.Any() ? Context.Services.Max(s => s.Id) + 1 : 1;

            CommonDataProviderFactory[] dataProviderFactories = CommonDataProviderFactory.CreateFactories();
            foreach (CommonDataProviderFactory factory in dataProviderFactories)
                factory.GetDataProvider().Save(User, service);

            Context.Services.Add(service);
        }


        public void Remove(Service service)
        {
            Context.Services.Remove(service);

            CommonDataProviderFactory[] dataProviderFactories = CommonDataProviderFactory.CreateFactories();
            foreach (CommonDataProviderFactory factory in dataProviderFactories)
                factory.GetDataProvider().Delete(User, service);
        }

        public void Update(int id, Service service)
        {
            Service tempEntity = Context.Services.First(c => c.Id == id);
            if (tempEntity != null)
            {
                tempEntity.Name = service.Name;
                tempEntity.Login = service.Login;
                tempEntity.Password = service.Password;
            }

            CommonDataProviderFactory[] dataProviderFactories = CommonDataProviderFactory.CreateFactories();
            foreach (CommonDataProviderFactory factory in dataProviderFactories)
                factory.GetDataProvider().Update(User, id, service);
        }
        public void Update(Service startEntity, Service finishEntity) => Update(startEntity.Id, finishEntity);
    }
}
