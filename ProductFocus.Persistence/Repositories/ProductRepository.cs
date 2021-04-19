using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public async Task<Product> GetById(long id)
        {
            return await _unitOfWork.GetAsync<Product>(id);
        }

    }
}
