using ProductFocus.Domain;
using ProductFocus.Domain.Model;
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
            Oragnizations = new OrganizationRepository(_context);
            Permissions = new PermissionRepository(_context);
            Products = new ProductRepository(_context);
            Roles = new RoleRepository(_context);
            Users = new UserRepository(_context);
        }

        public IFeatureRepository<Feature, long> Features { get; private set; }
        public IOrganizationRepository<Organization, long> Oragnizations { get; private set; }
        public IPermissionRepository<Permission, long> Permissions { get; private set; }
        public IProductRepository<Product, long> Products { get; private set; }
        public IRoleRepository<Role, long> Roles { get; private set; }
        public IUserRepository<User, long> Users { get; private set; }

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
