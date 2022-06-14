using ProductTests.Domain.Model.TestCaseAggregate;
using ProductTests.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure.Repositories
{
    public class TestCaseRepository : ITestCaseRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TestCaseRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void Add(TestCase testCase)
        {
            _unitOfWork.Insert(testCase);
        }

        public Task<TestCase> GetById(long id)
        {
            return _unitOfWork.GetAsync<TestCase>(id);
        }
    }
}
