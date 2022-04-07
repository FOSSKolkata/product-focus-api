using ProductTests.Domain.Model.TestPlanVersionAggregate;
using System.Threading.Tasks;

namespace ProductTests.Domain.Repositories
{
    public interface ITestPlanVersionRepository
    {
        void Add(TestPlanVersion testPlanVersion);
        Task<TestPlanVersion> GetById(long id);
    }
}
