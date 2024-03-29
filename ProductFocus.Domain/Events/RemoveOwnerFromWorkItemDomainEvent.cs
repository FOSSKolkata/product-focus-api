﻿using MediatR;
using ProductFocus.Domain.Model;

namespace ProductFocus.Domain.Events
{
    public class RemoveOwnerFromWorkItemDomainEvent : INotification
    {
        public Feature Feature { get; set; }
        public long EventTriggeredById { get; set; }
        public long ProductId { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public RemoveOwnerFromWorkItemDomainEvent(Feature feature, long eventTriggeredById, long productId, string ownerName, string ownerEmail)
        {
            Feature = feature;
            EventTriggeredById = eventTriggeredById;
            ProductId = productId;
            OwnerName = ownerName;
            OwnerEmail = ownerEmail;
        }
    }
}
