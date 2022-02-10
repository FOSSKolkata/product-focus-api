using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain;
using ProductFocus.Domain.Common;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductFocus.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductFocusDbContext _context;

        public UnitOfWork(ProductFocusDbContext context)
        {
            _context = context;
        }

        internal async Task<T> GetAsync<T>(long id)
            where T : class
        {
            return await _context.Set<T>().FindAsync(id);
        }

        internal async void InsertAsync<T>(T entity) 
            where T: class, IAggregateRoot
        {
            await _context.Set<T>().AddAsync(entity);
        }
        internal void Insert<T>(T entity)
            where T : class, IAggregateRoot
        {
            _context.Set<T>().Add(entity);
        }

        internal void Remove<T>(T entity)
            where T : class, IAggregateRoot
        {
            _context.Remove<T>(entity);
        }

        internal void Update<T>(T entity) 
            where T : class, IAggregateRoot
        {
            _context.Set<T>().Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
        }        
        internal IQueryable<T> Query<T>() 
            where T : class
        {
            return _context.Set<T>();
        }
        public async Task<int> CompleteAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveEntitiesAsync(cancellationToken);
        }

        public async void Dispose()
        {
            await _context.DisposeAsync();
            GC.SuppressFinalize(this);
        }
    }
}
