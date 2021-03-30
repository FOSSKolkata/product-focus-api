using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class Repository<TEntity, TId> : IRepository<TEntity, TId> where TEntity : AggregateRoot<TId>
    {
        private readonly DbContext _context;
        private readonly DbSet<TEntity> _dbSet;
        public Repository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }
        public virtual void Delete(TId id, string currentUserId)
        {
            TEntity entityToDelete = _dbSet.Find(id);
            Delete(entityToDelete, currentUserId);
        }

        public virtual void Delete(TEntity entityToDelete, string currentUserId)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                _dbSet.Attach(entityToDelete);
            }

            // TODO : Implement soft delete
            if (entityToDelete is ISoftDeletable softDeletable)
            {
                softDeletable.IsDeleted = true;
                softDeletable.DeletedOn = DateTime.UtcNow;
                softDeletable.DeletedBy = currentUserId;
            }
            else
            {
                _dbSet.Remove(entityToDelete);
            }
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbSet.ToList();
        }

        public IEnumerable<TEntity> GetAllWith(Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            params Expression<Func<TEntity, object>>[] includes)
        {
            IQueryable<TEntity> query = _dbSet;

            if(filter != null)
            {
                query = query.Where(filter);
            }
            if (includes.Length > 0)
            {
                query = query.Include(includes[0]);
            }
            for (int queryIndex = 1; queryIndex < includes.Length; ++queryIndex)
            {
                query = query.Include(includes[queryIndex]);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }            
        }

        public TEntity GetById(TId id)
        {
            return _dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            _dbSet.Add(entity);
            //DispatchDomainEvents(entity);           
        }

        public virtual void Update(TEntity entity)
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;

            //DispatchDomainEvents(entity);
        }
    }
}
