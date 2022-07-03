using ProductFocus.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface ISprintRepository
    {
        void AddSprint(Sprint sprint);
        Sprint GetByName(string name);
        Task<Sprint> GetById(long id);
        Task<List<Sprint>> GetByProductId(long productId);
    }
}
