using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Repositories
{
    public interface IBusinessRequirementRepository
    {
        void Add(BusinessRequirement businessRequirement);
    }
}
