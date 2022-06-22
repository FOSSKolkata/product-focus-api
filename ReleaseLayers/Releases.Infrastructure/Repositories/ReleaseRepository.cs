using Releases.Domain.Model;
using Releases.Domain.Repositories;

namespace Releases.Infrastructure.Repositories
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
