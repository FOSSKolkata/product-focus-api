using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IModuleRepository
    {
        Task<Module> GetById(long id);
        Task<Module> GetByName(string name);
    }
}
