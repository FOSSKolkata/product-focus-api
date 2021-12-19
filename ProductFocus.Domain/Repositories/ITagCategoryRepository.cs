using ProductFocus.Domain.Model;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Repositories
{
    public interface ITagCategoryRepository
    {
        void AddTagCategory(TagCategory tagCategory);
        Task<TagCategory> GetById(long id);
    }
}
