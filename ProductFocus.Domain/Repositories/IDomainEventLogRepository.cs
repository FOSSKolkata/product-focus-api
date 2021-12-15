using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Repositories
{
    public interface IDomainEventLogRepository
    {
        void AddDomainEventLog(WorkItemDomainEventLog domainEventLog);
    }
}
