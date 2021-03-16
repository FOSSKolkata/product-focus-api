using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public interface IRepository<TEntity, TId> where TEntity: AggregateRoot<TId>
    {
        IEnumerable<TEntity> GetAll();
        TEntity GetById(TId id);
        void Insert(TEntity entity);
        void Update(TEntity entity);
        void Delete(TId id);
        void Delete(TEntity id);
        IQueryable<TEntity>  GetAllWith(params Expression<Func<TEntity, object>>[] includes);
    }
}
