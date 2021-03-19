using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class RoleRepository : Repository<Role, long>, IRoleRepository
    {
        public RoleRepository(ProductFocusDbContext context) : base(context)
        {

        }

    }
}
