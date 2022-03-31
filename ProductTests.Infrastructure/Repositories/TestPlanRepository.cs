using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure.Repositories
{
    public class TestPlanRepository : ITestPlanRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public TestPlanRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(TestPlan testPlan)
        {
            _unitOfWork.Insert(testPlan);
        }
        public Task<TestPlan> GetById(long id)
        {
            return _unitOfWork.GetAsync<TestPlan>(id);
        }
    }
}
