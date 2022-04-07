using ProductTests.Domain.Model.TestCaseVersionAggregate;
using System.Threading.Tasks;

namespace ProductTests.Domain.Repositories
{
    public interface ITestCaseVersionRepository
    {
        void Add(TestCaseVersion testCaseVersion);
        Task<TestCaseVersion> GetById(long id);
    }
}
