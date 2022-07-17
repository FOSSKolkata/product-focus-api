using EventBus.Abstractions;

namespace ProductFocusApi.IntegrationEvents.Services
{
    public class ProductFocusEventBusOwningService : IEventBusOwningService
    {
        public string Name => "ProductFocus";
    }
}
