using ProductDocumentations.Domain.Model;

namespace ProductDocumentations.Domain.Repositories
{
    public interface IProductDocumentationRepository
    {
        void Add(ProductDocumentation productDocumentation);
    }
}
