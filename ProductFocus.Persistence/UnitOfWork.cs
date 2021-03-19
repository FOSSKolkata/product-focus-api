using ProductFocus.Domain;
using ProductFocus.Domain.Repositories;
using ProductFocus.Persistence.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ProductFocusDbContext _context;

        public UnitOfWork(ProductFocusDbContext context)
        {
            _context = context;
            Features = new FeatureRepository(_context);
        }

        public IFeatureRepository Features { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
