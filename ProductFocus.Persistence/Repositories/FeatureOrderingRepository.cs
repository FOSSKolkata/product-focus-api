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
    public class FeatureOrderingRepository : IFeatureOrderingRepository
    {
        private UnitOfWork _unitOfWork;
        public FeatureOrderingRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(FeatureOrdering featureOrdering)
        {
            _unitOfWork.Insert<FeatureOrdering>(featureOrdering);
        }
        public async Task<FeatureOrdering> GetByFeatureIdAndCategory(long id, long sprintId, OrderingCategoryEnum orderingCategoryEnum)
        {
            return await _unitOfWork.Query<FeatureOrdering>().Where(x => x.FeatureId == id && x.OrderingCategory == orderingCategoryEnum && x.SprintId == sprintId).SingleOrDefaultAsync();
        }

        public async Task<List<FeatureOrdering>> GetByCategoryAndSprint(OrderingCategoryEnum orderingCategoryEnum, long sprintId)
        {
            return await _unitOfWork.Query<FeatureOrdering>().Where(x => x.OrderingCategory == orderingCategoryEnum && x.SprintId == sprintId).ToListAsync();
        }

        public void Remove(FeatureOrdering featureOrdering)
        {
            _unitOfWork.Remove(featureOrdering);
        }

        public async Task<List<FeatureOrdering>> GetByIdAndSprint(long id, long sprintId)
        {
            return await _unitOfWork.Query<FeatureOrdering>().Where(x => x.FeatureId == id && x.SprintId == sprintId).ToListAsync();
        }
    }
}
