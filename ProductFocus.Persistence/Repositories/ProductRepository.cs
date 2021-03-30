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
    public class ProductRepository : IProductRepository<Product, long>
    {
        private readonly DbContext _context;
        public ProductRepository(ProductFocusDbContext context)
        {
            _context = context;
        }

    }
}
