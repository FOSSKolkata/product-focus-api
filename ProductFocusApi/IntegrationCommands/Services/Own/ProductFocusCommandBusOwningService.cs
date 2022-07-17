using CommandBus.Abstractions;

namespace ProductFocusApi.IntegrationCommands.Services.Own
{
    public class ProductFocusCommandBusOwningService : IOwningService
    {
        public string Name => "ProductFocus";
    }
}
