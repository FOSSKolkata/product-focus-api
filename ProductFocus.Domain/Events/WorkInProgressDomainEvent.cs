using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class WorkInProgressDomainEvent : INotification
    {
        public Feature Feature { get; }
        public long EventTriggeredById { get; }
        public long ProductId { get; }
        public long OldWorkPercentage { get; }
        public long NewWorkPercentage { get; }
        public WorkInProgressDomainEvent(Feature feature, long eventTriggeredById, long productId, long oldWorkPercentage, long newWorkPercentage)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            OldWorkPercentage = oldWorkPercentage;
            NewWorkPercentage = newWorkPercentage;
        }
    }
}
