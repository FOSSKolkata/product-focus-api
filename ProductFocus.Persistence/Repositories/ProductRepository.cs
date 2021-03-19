using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class ProductRepository : Repository<Product, long>, IProductRepository
    {
        public ProductRepository(ProductFocusDbContext context) : base(context)
        {

        }

    }
}
