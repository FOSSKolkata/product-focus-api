using Microsoft.EntityFrameworkCore;
using ProductFocus.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TagRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void AddTag(Tag tag)
        {
            _unitOfWork.Insert<Tag>(tag);
        }
        public async Task<Tag> GetById(long id)
        {
            return await _unitOfWork.GetAsync<Tag>(id);
        }
        public async Task<Tag> GetByNameAndProductId(string name, long productId)
        {
            return await _unitOfWork.Query<Tag>().Where(x => x.ProductId == productId && x.Name == name).SingleOrDefaultAsync();
        }
    }
}
