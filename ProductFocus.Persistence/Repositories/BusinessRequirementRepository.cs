using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Threading.Tasks;

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
            _unitOfWork.Insert(businessRequirement);
        }

        public Task<BusinessRequirement> GetById(long id)
        {
            return _unitOfWork.GetAsync<BusinessRequirement>(id);
        }
    }
}
