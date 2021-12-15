using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IFeatureRepository
    {
        void AddFeature(Feature feature);
        Task<Feature> GetById(long id);
    }
}
