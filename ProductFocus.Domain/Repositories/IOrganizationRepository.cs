using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IOrganizationRepository
    {
        void AddOrganization(Organization organization);
        Task<Organization> GetById(long id);
        Organization GetByName(string name);
    }
}
