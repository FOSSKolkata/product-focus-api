using BusinessRequirements.Domain.Model;

namespace BusinessRequirements.Domain.Repositories
{
    public interface IProductRepository
    {
        void Add(Product product);
        Task<Product> GetById(long id);
    }
}
