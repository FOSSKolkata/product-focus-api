using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class AddWorkItemDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long SprintId { get; set; }
        public long ProductId { get; set; }
        public AddWorkItemDomainEvent(Feature feature, long eventTriggeredById, long sprintId, long productId)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            SprintId = sprintId;
            ProductId = productId;
        }

    }
}
