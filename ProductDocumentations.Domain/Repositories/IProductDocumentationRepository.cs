using ProductDocumentations.Domain.Model;
using System.Threading.Tasks;

namespace ProductDocumentations.Domain.Repositories
{
    public interface IProductDocumentationRepository
    {
        void Add(ProductDocumentation productDocumentation);
        Task<ProductDocumentation> GetById(long id);
    }
}
