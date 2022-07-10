using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Infrastructure.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public ProductRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Product product)
        {
            _unitOfWork.Insert(product);
        }

        public Task<Product> GetById(long id)
        {
            return _unitOfWork.GetAsync<Product>(id);
        }
    }
}
