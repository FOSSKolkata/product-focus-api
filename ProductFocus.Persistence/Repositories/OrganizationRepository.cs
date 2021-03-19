using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class OrganizationRepository : Repository<Organization, long>, IOrganizationRepository
    {
        public OrganizationRepository(ProductFocusDbContext context) : base(context)
        {

        }

    }
}
