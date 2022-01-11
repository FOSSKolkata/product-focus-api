using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;

namespace ProductFocus.Persistence.Repositories
{
    public class BusinessRequirementRepository : IBusinessRequirementRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public BusinessRequirementRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(BusinessRequirement businessRequirement)
        {
            _unitOfWork.Insert<BusinessRequirement>(businessRequirement);
        }
    }
}
