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
        public WorkItemDomainEventLog(string eventTypeName, string domainEventJson, long? moduleId, string? moduleName, long eventTriggedById, string eventTriggedBy, long productId, long featureId)
        {
            EventTypeName = eventTypeName;
            DomainEventJson = domainEventJson;
            ModuleId = moduleId;
            ModuleName = moduleName;
            CreatedOn = DateTime.UtcNow;
            CreatedBy = eventTriggedBy;
            CreatedById = eventTriggedById;
            ProductId = productId;
            FeatureId = featureId;
        }

        public string EventTypeName { get; private set; }
        public string DomainEventJson { get; private set; }
        public virtual long? ModuleId { get; private set; }
        public virtual long ProductId { get; private set; }
        public virtual long FeatureId { get; private set; }
        public string ModuleName { get; private set; }
    }
}
