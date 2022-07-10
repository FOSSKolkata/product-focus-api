using BusinessRequirements.Domain.Model.BusinessAggregate;
using BusinessRequirements.Domain.Repositories;
using BusinessRequirements.Infrastructure;

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
            _unitOfWork.Insert(businessRequirementTag);
        }
        public Task<BusinessRequirementTag> GetById(long id)
        {
            return _unitOfWork.GetAsync<BusinessRequirementTag>(id);
        }
    }
}
