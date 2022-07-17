using EventBus.Events;

namespace ProductFocusApi.IntegrationEvents.Events
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
    }
}
