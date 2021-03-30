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
        void Delete(TId id, string currentUserId);
        void Delete(TEntity id, string currentUserId);
        IEnumerable<TEntity> GetAllWith(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes);
    }
}
