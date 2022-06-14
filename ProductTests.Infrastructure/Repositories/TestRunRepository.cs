using ProductTests.Domain.Model.TestRunAggregate;
using ProductTests.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure.Repositories
{
    public class TestRunRepository : ITestRunRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TestRunRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(TestRun testRun)
        {
            _unitOfWork.Insert(testRun);
        }

        public async Task<TestRun> GetById(long id)
        {
            return await _unitOfWork.GetAsync<TestRun>(id);
        }
    }
}
