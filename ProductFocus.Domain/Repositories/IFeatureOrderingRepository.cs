using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IFeatureOrderingRepository
    {
        void Add(FeatureOrdering featureOrdering);
        Task<FeatureOrdering> GetByFeatureIdAndCategory(long id, long sprintId, OrderingCategoryEnum orderingCategoryEnum);
        Task<List<FeatureOrdering>> GetByCategoryAndSprint(OrderingCategoryEnum orderingCategoryEnum, long sprintId);
        Task<List<FeatureOrdering>> GetByIdAndSprint(long id, long sprintId);
        void Remove(FeatureOrdering featureOrder);
    }
}
