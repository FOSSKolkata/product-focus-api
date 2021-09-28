using MediatR;
using ProductFocus.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductFocus.Domain.Events
{
    public class AddOwnerToWorkItemDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }

        public AddOwnerToWorkItemDomainEvent(Feature feature, long eventTriggeredById, long productId, string ownerName, string ownerEmail) 
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            OwnerName = ownerName;
            OwnerEmail = ownerEmail;
        }
    }
}
