using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        //IFeatureRepository<Feature, long> Features { get;}
        //IOrganizationRepository<Organization, long> Oragnizations { get; }
        //IPermissionRepository<Permission, long> Permissions { get; }
        //IProductRepository<Product, long> Products { get; }
        //IRoleRepository<Role, long> Roles { get; }
        //IUserRepository<User, long> Users { get; }
        Task<int> CompleteAsync();
    }
}
