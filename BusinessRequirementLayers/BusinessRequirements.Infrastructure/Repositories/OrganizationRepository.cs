using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Infrastructure.Repositories
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public OrganizationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Organization organization)
        {
            _unitOfWork.Insert(organization);
        }

        public Task<Organization> GetById(long id)
        {
            return _unitOfWork.GetAsync<Organization>(id);
        }
    }
}
