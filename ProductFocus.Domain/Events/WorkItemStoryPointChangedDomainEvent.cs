using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class WorkItemStoryPointChangedDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public long? PreviousStoryPoint { get; set; }
        public long CurrentStoryPoint { get; set; }
        public WorkItemStoryPointChangedDomainEvent(Feature feature, long eventTriggeredById, long productId, long? previousStoryPoint, long currentStoryPoint)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousStoryPoint = previousStoryPoint;
            CurrentStoryPoint = currentStoryPoint;
        }
    }
}
