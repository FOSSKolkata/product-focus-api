using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
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
            _unitOfWork.Insert<Feature>(feature);
        }

        public async Task<Feature> GetById(long id)
        {
            return await _unitOfWork.GetAsync<Feature>(id);
        }
    }
}
