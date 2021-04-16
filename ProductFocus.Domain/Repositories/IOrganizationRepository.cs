using Common;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IOrganizationRepository
    {
        void AddOrganization(Organization organization);
        Task<Organization> GetById(long id);
    }
}
