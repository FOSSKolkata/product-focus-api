using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class WorkItemDescriptionChangedDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById {get; set;}
        public long ProductId { get; set; }
        public string PreviousDescription { get; set; }
        public string CurrentDescription { get; set; }
        public WorkItemDescriptionChangedDomainEvent(Feature feature, long eventTriggeredById, long productId, string previousDescription, string currentDescription)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousDescription = previousDescription;
            CurrentDescription = currentDescription;
        }
    }
}
