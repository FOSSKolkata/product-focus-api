using ProductFocus.Domain.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IFeatureOrderingRepository
    {
        void Add(FeatureOrdering featureOrdering);
        Task<FeatureOrdering> GetByFeatureIdAndCategory(long id, long sprintId);
        Task<List<FeatureOrdering>> GetByCategoryAndSprint(long sprintId);
        Task<List<FeatureOrdering>> GetByIdAndSprint(long id, long sprintId);
        void Remove(FeatureOrdering featureOrder);
    }
}
