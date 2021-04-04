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
    public class OrganizationRepository : IOrganizationRepository<Organization, long>
    {
        private readonly UnitOfWork _unitOfWork;

        public OrganizationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
