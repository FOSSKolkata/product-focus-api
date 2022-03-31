using ProductTests.Domain.Model.TestCaseAggregate;
using System.Threading.Tasks;

namespace ProductTests.Domain.Repositories
{
    public interface ITestCaseRepository
    {
        void Add(TestCase testCase);
        Task<TestCase> GetById(long id);
    }
}
