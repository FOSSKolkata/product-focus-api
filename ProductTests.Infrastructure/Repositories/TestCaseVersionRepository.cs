using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure.Repositories
{
    public class TestCaseVersionRepository : ITestCaseVersionRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TestCaseVersionRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(TestCaseVersion testCaseVersion)
        {
            _unitOfWork.Insert(testCaseVersion);
        }

        public async Task<TestCaseVersion> GetById(long id)
        {
            return await _unitOfWork.GetAsync<TestCaseVersion>(id);
        }
    }
}
