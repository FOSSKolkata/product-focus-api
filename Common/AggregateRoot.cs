using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class AggregateRoot<TId> : Entity<TId>, IDomainEventManager
    {

        private readonly List<IDomainEvent> _domainEvents;

        [NotMapped]
        public IReadOnlyList<IDomainEvent> DomainEvents
        {
            get
            {
                return _domainEvents;
            }
            private set { }
        }

        public AggregateRoot()
        {
            _domainEvents = new List<IDomainEvent>();
        }

        protected void AddDomainEvent(IDomainEvent newEvent)
        {
            _domainEvents.Add(newEvent);
        }

        public virtual void ClearEvents()
        {
            _domainEvents.Clear();
        }
    }
}
