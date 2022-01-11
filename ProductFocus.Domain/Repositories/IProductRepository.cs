using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IProductRepository
    {
        void AddProduct(Product product);
        Task<Product> GetById(long id);
        Task<Module> GetModuleById(long id);
    }
}
