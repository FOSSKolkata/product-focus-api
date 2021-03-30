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
    public class FeatureRepository : IFeatureRepository<Feature, long>
    {
        private readonly DbContext _context;
        public FeatureRepository(ProductFocusDbContext context)
        {
            _context = context;
        }

    }
}
