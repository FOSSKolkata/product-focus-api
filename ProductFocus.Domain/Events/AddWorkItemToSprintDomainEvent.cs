using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class AddWorkItemToSprintDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public string PreviousSprint { get; set; }
        public string CurrentSprint { get; set; }
        public AddWorkItemToSprintDomainEvent(Feature feature, long eventTriggeredById, long productId, string previousSprint, string currentSprint)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            PreviousSprint = previousSprint;
            CurrentSprint = currentSprint;
        }
    }
}
