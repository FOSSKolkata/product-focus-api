using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using ProductFocusApi.IntegrationEvents.Services;
using Releases.Application.IntegrationEvents.Events;
using Releases.Application.IntegrationEvents.Services;

namespace ProductFocusApi.StartUp
{
    public static class ConfigureApp
    {
        public static IApplicationBuilder AddProductFocus(this IApplicationBuilder app)
        {
            /*var commandBus = app.ApplicationServices.GetRequiredService<ICommandBus<ProductFocusOwningService>>();
            commandBus.Subscribe<RunWorkflowIntegrationCommand, RunWorkflowIntegrationCommandHandler>();
            commandBus.Subscribe<AddPatientAlertCriterionIntegrationCommand, AddPatientAlertCriterionIntegrationCommandHandler>();*/

            //var eventBus = app.ApplicationServices.GetRequiredService<IEventBus<ProductFocusEventBusOwningService>>();
            //eventBus.Subscribe<PatientAlertCriteriaChangedForSourceDataIntegrationEvent, PatientAlertCriteriaChangedForSourceDataIntegrationEventHandler>();

            return app;
        }
    }
}
