using ProductTests.Domain.Model.PublishedTestCaseAggregate;
using ProductTests.Domain.Repositories;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure.Repositories
{
    public class PublishedTestCaseRepository : IPublishedTestCaseRepository
    {
        private readonly UnitOfWork _unitOfWork;
        public PublishedTestCaseRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public void Add(PublishedTestCase testCase)
        {
            _unitOfWork.Insert(testCase);
        }

        public Task<PublishedTestCase> GetById(long id)
        {
            return _unitOfWork.GetAsync<PublishedTestCase>(id);
        }
    }
}
