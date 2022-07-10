using BusinessRequirements.Domain.Model;

namespace BusinessRequirements.Domain.Repositories
{
    public interface IBusinessRequirementRepository
    {
        void Add(BusinessRequirement businessRequirement);
        Task<BusinessRequirement> GetById(long id);
    }
}
