using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IReleaseRepository
    {
        void Add(Release release);
        Task<Release> GetById(long id);
    }
}
