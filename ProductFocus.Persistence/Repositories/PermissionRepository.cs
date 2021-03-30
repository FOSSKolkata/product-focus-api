using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class PermissionRepository : IPermissionRepository<Permission, long>
    {
        private readonly DbContext _context;
        public PermissionRepository(ProductFocusDbContext context)
        {
            _context = context;
        }

    }

}
