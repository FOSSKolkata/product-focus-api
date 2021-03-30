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
    public class UserRepository : IUserRepository<User, long>
    {
        private readonly DbContext _context;
        public UserRepository(ProductFocusDbContext context)
        {
            _context = context;
        }

    }
}
