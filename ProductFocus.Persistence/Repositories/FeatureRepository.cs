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
    public class FeatureRepository : IFeatureRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public FeatureRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddFeature(Feature feature)
        {
            _unitOfWork.InsertAsync<Feature>(feature);
        }
    }
}
