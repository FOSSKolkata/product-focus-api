using ProductTests.Domain.Model.TestPlanVersionAggregate;
using ProductTests.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure.Repositories
{
    public class TestPlanVersionRepository : ITestPlanVersionRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TestPlanVersionRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(TestPlanVersion testPlanVersion)
        {
            _unitOfWork.InsertAsync(testPlanVersion);
        }

        public async Task<TestPlanVersion> GetById(long id)
        {
            return await _unitOfWork.GetAsync<TestPlanVersion>(id);
        }
    }
}
