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
    public class FeatureOrderRepository : IFeatureOrderRepository
    {
        private UnitOfWork _unitOfWork;
        public FeatureOrderRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(FeatureOrdering featureOrder)
        {
            _unitOfWork.Insert<FeatureOrdering>(featureOrder);
        }
        public async Task<FeatureOrdering> GetByFeatureIdAndCategory(long id, long sprintId, OrderingCategoryEnum order)
        {
            return await _unitOfWork.Query<FeatureOrdering>().Where(x => x.FeatureId == id && x.OrderingCategory == order && x.SprintId == sprintId).SingleOrDefaultAsync();
        }

        public async Task<List<FeatureOrdering>> GetByCategoryAndSprint(OrderingCategoryEnum order, long sprintId)
        {
            return await _unitOfWork.Query<FeatureOrdering>().Where(x => x.OrderingCategory == order && x.SprintId == sprintId).ToListAsync();
        }

        public void Remove(FeatureOrdering featureOrder)
        {
            _unitOfWork.Remove(featureOrder);
        }
    }
}
