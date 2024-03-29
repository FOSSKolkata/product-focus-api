﻿using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public ProductRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddProduct(Product product)
        {
            _unitOfWork.Insert<Product>(product);
        }

        public async Task<Product> GetById(long id)
        {
            return await _unitOfWork.GetAsync<Product>(id);
        }

        public async Task<Module> GetModuleById(long id)
        {
            return await _unitOfWork.GetAsync<Module>(id);
        }
    }
}
