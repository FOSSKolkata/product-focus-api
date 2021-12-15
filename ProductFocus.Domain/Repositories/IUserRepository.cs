using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Repositories
{
    public interface IUserRepository
    {
        void RegisterUser(User user);
        User GetByEmail(string name);
        User GetByIdpUserId(string ipdUserId);
        User GetById(long id);
    }
}
