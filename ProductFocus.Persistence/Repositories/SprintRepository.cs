using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace ProductFocus.Persistence.Repositories
{
    public class SprintRepository : ISprintRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public SprintRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void AddSprint(Sprint sprint)
        {
            _unitOfWork.Insert<Sprint>(sprint);
        }

        public Sprint GetByName(string name)
        {
            return _unitOfWork.Query<Sprint>().SingleOrDefault(x => x.Name == name);
        }

        public async Task<Sprint> GetById(long id)
        {
            return await _unitOfWork.GetAsync<Sprint>(id);
        }
    }
}
