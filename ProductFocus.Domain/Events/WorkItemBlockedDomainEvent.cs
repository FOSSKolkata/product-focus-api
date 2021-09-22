using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class WorkItemBlockedDomainEvent : INotification
    {
        public Feature Feature { get; }
        public long EventTriggeredById { get; }
        public long ProductId { get; }

        public WorkItemBlockedDomainEvent(Feature feature, long eventTriggedById, long productId)
        {
            Feature = feature;
            EventTriggeredById = eventTriggedById;
            ProductId = productId;
        }
    }
}
