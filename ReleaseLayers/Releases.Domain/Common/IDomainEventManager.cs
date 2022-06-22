using MediatR;
using System.Collections.Generic;

namespace Releases.Domain.Common
{
    public interface IDomainEventManager
    {
        IReadOnlyCollection<INotification> DomainEvents { get; }

        void AddDomainEvent(INotification eventItem);

        void RemoveDomainEvent(INotification eventItem);

        void ClearDomainEvents();
    }
}
