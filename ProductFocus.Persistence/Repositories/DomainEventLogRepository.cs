using ProductFocus.Domain.Model;
using ProductFocus.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
