using MediatR;
using ProductFocus.Domain.Model;
using System;

namespace ProductFocus.Domain.Events
{
    public class WorkItemEndDateChangedDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public DateTime? PreviousEndDate { get; set; }
        public DateTime CurrentEndDate { get; set; }
        public WorkItemEndDateChangedDomainEvent(Feature feature, long eventTriggeredById, long productId, DateTime? previousEndDate, DateTime currentEndDate)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousEndDate = previousEndDate;
            CurrentEndDate = currentEndDate;
        }
    }
}
