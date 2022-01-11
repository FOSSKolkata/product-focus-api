using ProductFocus.Domain.Model.BusinessAggregate;

namespace ProductFocus.Domain.Repositories
{
    public interface IBusinessRequirementTagRepository
    {
        void Add(BusinessRequirementTag businessRequirementTag);
    }
}
