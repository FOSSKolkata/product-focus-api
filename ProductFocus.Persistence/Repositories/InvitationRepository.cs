using ProductFocus.Domain;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class InvitationRepository : IInvitationRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public InvitationRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddInvitation(Invitation invitation)
        {
            _unitOfWork.Insert<Invitation>(invitation);
        }

        public Invitation GetActiveInvitation(Organization organization, string email)
        {         
            return _unitOfWork.Query<Invitation>()
                .Where(y => y.Status == InvitationStatus.New || y.Status == InvitationStatus.Resent)
                .SingleOrDefault(x => x.Email == email && x.Organization == organization);
        }
    }
}
