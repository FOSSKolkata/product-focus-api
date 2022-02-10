using ProductFocus.Domain.Model.BusinessAggregate;
using ProductFocus.Domain.Repositories;

namespace ProductFocus.Persistence.Repositories
{
    public class BusinessRequirementTagRepository : IBusinessRequirementTagRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public BusinessRequirementTagRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(BusinessRequirementTag businessRequirementTag)
        {
            _unitOfWork.Insert<BusinessRequirementTag>(businessRequirementTag);
        }
    }
}
