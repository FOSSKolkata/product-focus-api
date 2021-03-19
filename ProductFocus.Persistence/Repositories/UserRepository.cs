using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class UserRepository : Repository<User, long>, IUserRepository
    {
        public UserRepository(ProductFocusDbContext context) : base(context)
        {

        }

    }
}
