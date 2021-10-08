using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IFeatureOrderRepository
    {
        void Add(FeatureOrdering featureOrder);
        Task<FeatureOrdering> GetByFeatureIdAndCategory(long id, long sprintId, OrderingCategoryEnum order);
        void Remove(FeatureOrdering featureOrder);
    }
}
