using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class ReleaseRepository : IReleaseRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public ReleaseRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(Release release)
        {
            _unitOfWork.Insert(release);
        }

        public Task<Release> GetById(long id)
        {
            return _unitOfWork.GetAsync<Release>(id);
        }
    }
}
