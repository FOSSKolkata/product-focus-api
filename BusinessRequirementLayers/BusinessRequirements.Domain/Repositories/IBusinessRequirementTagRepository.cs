
using BusinessRequirements.Domain.Model.BusinessAggregate;

namespace BusinessRequirements.Domain.Repositories
{
    public interface IBusinessRequirementTagRepository
    {
        void Add(BusinessRequirementTag businessRequirementTag);
        Task<BusinessRequirementTag> GetById(long id);
    }
}
