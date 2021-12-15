using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;

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
    }
}
