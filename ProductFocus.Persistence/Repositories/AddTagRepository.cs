using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;

namespace ProductFocus.Persistence.Repositories
{
    public class AddTagRepository : ITagRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public AddTagRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddTag(Tag tag)
        {
            _unitOfWork.Insert<Tag>(tag);
        }
    }
}
