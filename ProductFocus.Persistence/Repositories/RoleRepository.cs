using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
