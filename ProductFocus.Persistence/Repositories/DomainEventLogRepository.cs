using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;

namespace ProductFocus.Persistence.Repositories
{
    public class DomainEventLogRepository : IDomainEventLogRepository
    {
        private readonly UnitOfWork _unitOfWork;

        public DomainEventLogRepository(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public void AddDomainEventLog(WorkItemDomainEventLog domainEventLog)
        {
            _unitOfWork.Insert<WorkItemDomainEventLog>(domainEventLog);
        }
    }
}
