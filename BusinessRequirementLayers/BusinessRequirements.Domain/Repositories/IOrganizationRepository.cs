using BusinessRequirements.Domain.Model;

namespace BusinessRequirements.Domain.Repositories
{
    public interface IOrganizationRepository
    {
        void Add(Organization organization);
        Task<Organization> GetById(long id);
    }
}
