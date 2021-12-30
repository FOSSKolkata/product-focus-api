using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface ITagRepository
    {
        void AddTag(Tag tag);
        Task<Tag> GetById(long id);
        Task<Tag> GetByNameAndProductId(string name, long productId);
    }
}
