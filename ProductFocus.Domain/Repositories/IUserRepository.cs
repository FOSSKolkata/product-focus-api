using System;
using Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Repositories
{
    public interface IUserRepository
    {
        void RegisterUser(User user);
        User GetByEmail(string name);
        User GetByIdpUserId(string ipdUserId);
    }
}
