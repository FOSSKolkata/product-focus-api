using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Model
{
    public class WorkItemDomainEventLog : AggregateRoot<Guid> 
    {

        private WorkItemDomainEventLog() { }
        public WorkItemDomainEventLog(string eventTypeName, string domainEventJson, long moduleId, string eventTriggedBy)
        {
            EventTypeName = eventTypeName;
            DomainEventJson = domainEventJson;
            ModuleId = moduleId;
            CreatedOn = DateTime.UtcNow;
            CreatedBy = eventTriggedBy;
        }

        public string EventTypeName { get; private set; }
        public string DomainEventJson { get; private set; }
        public virtual long ModuleId { get; private set; }

    }
}
