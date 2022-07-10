using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class BusinessRequirementWorkItemLinkRepository : IBusinessRequirementWorkItemLinkRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public BusinessRequirementWorkItemLinkRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(BusinessRequirementToWorkItemLink businessRequirementToWorkItemLink)
        {
            _unitOfWork.Insert(businessRequirementToWorkItemLink);
        }

        public Task<BusinessRequirementToWorkItemLink> GetById(long id)
        {
            return _unitOfWork.GetAsync<BusinessRequirementToWorkItemLink>(id);
        }
    }
}
