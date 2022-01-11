using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface IInvitationRepository
    {
        void AddInvitation(Invitation invitation);
        Invitation GetActiveInvitation(Organization organization, string email);
        Task<Invitation> GetById(long id);
    }
}
