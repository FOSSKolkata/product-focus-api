using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class PermissionRepository : Repository<Permission, long>, IPermissionRepository
    {
        public PermissionRepository(ProductFocusDbContext context) : base(context)
        {

        }

    }
}
