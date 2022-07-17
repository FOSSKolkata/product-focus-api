using EventBus.Abstractions;
using EventBus.Events;
using IntegrationEventLogEF;
using Releases.Application.IntegrationEvents.Services;
using Releases.Domain.Common;

namespace Releases.Application.IntegrationEvents.Events
{
    public record OrganizationAddedIntegrationEvent : IntegrationEvent
    {
        public long OrganizationId { get; set; }
        public string OrganizationName { get; set; }
        public OrganizationAddedIntegrationEvent(long organizationId, string organizationName)
        {
            OrganizationId = organizationId;
            OrganizationName = organizationName;
        }

        internal class OrganizationAddedIntegrationEventHandler : BaseIntegrationEventHandler<OrganizationAddedIntegrationEvent>,
            IIntegrationEventHandler<OrganizationAddedIntegrationEvent>
        {
            private readonly IUnitOfWork _unitOfWork;
            public OrganizationAddedIntegrationEventHandler(IReleaseIncomingIntegrationEventLogService integrationEventLogService,
                IUnitOfWork unitOfWork)
                : base(integrationEventLogService)
            {
                _unitOfWork = unitOfWork;
            }

            public override Task Handle(OrganizationAddedIntegrationEvent @event)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
