using ProductFocus.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;

namespace ProductFocus.Persistence.Repositories
{
    public class RoleRepository : IRoleRepository<Role, long>
    {
        private readonly UnitOfWork _unitOfWork;

        public RoleRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

    }
}
