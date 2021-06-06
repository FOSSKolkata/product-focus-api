using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IInvitationRepository
    {
        void AddInvitation(Invitation invitation);
        Invitation GetByEmail(string email);
    }
}
