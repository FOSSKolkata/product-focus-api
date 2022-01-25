using ProductFocus.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class TagCategoryRepository : ITagCategoryRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TagCategoryRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddTagCategory(TagCategory tagCategory)
        {
            _unitOfWork.Insert<TagCategory>(tagCategory);
        }

        public Task<TagCategory> GetById(long id)
        {
            return _unitOfWork.GetAsync<TagCategory>(id);
        }
    }
}
