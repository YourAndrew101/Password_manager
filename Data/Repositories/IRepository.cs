using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(int id);
        ICollection<TEntity> GetAll();

        bool Any();

        void Add(TEntity entity);

        void Update(int id, TEntity entity);
        void Update(TEntity startEntity, TEntity finishEntity);

        void Remove(TEntity entity);
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}
