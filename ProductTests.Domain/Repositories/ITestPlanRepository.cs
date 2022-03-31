using ProductTests.Domain.Model.TestPlanAggregate;
using System.Threading.Tasks;

namespace ProductTests.Domain.Repositories
{
    public interface ITestPlanRepository
    {
        void Add(TestPlan testPlan);
        Task<TestPlan> GetById(long id);
    }
}
