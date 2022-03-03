using ProductDocumentations.Domain.Model;
using ProductDocumentations.Domain.Repositories;
using System;
using System.Threading.Tasks;

namespace ProductDocumentations.Infrastructure.Repositories
{
    public class ProductDocumentationRepository : IProductDocumentationRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductDocumentationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(ProductDocumentation productDocumentation)
        {
            _unitOfWork.Insert(productDocumentation);
        }

        public async Task<ProductDocumentation> GetById(long id)
        {
            return await _unitOfWork.GetAsync<ProductDocumentation>(id);
        }
    }
}
