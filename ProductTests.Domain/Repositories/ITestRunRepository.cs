using ProductTests.Domain.Model.TestRunAggregate;
using System.Threading.Tasks;

namespace ProductTests.Domain.Repositories
{
    public interface ITestRunRepository
    {
        void Add(TestRun testRun);
        Task<TestRun> GetById(long id);
    }
}
