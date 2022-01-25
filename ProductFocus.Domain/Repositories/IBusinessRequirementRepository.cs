using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IBusinessRequirementRepository
    {
        void Add(BusinessRequirement businessRequirement);
        Task<BusinessRequirement> GetById(long id);
    }
}
