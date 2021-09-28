using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class WorkItemStartDateChangedDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public DateTime? PreviousStartDate { get; set; }
        public DateTime CurrentStartDate { get; set; }
        public WorkItemStartDateChangedDomainEvent(Feature feature, long eventTriggeredById, long productId, DateTime? previousStartDate, DateTime currentStartDate)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousStartDate = previousStartDate;
            CurrentStartDate = currentStartDate;
        }
    }
}
