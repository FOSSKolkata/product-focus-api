using Common;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IFeatureRepository
    {
        void AddFeature(Feature feature);
        Task<Feature> GetById(long id);
    }
}
