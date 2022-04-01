using ProductTests.Domain.Model.PublishedTestCaseAggregate;
using System.Threading.Tasks;

namespace ProductTests.Domain.Repositories
{
    public interface IPublishedTestCaseRepository
    {
        void Add(PublishedTestCase testCase);
        Task<PublishedTestCase> GetById(long id);
    }
}
