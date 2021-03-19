using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain
{
    public interface IUnitOfWork : IDisposable
    {
        IFeatureRepository Features { get;}
        int Complete();
    }
}
