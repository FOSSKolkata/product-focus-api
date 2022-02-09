using MediatR;
using System.Collections.Generic;

namespace ProductDocumentations.Domain.Common
{
    public interface IDomainEventManager
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }

        void AddDomainEvent(INotification eventItem);

        void RemoveDomainEvent(INotification eventItem);

        void ClearDomainEvents();
    }
}
