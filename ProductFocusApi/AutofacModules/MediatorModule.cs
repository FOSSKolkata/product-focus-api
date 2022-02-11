using Autofac;
using MediatR;
using ProductFocusApi.DomainEventHandlers;
using ProductFocusApi.QueryHandlers;
using System.Reflection;

namespace ProductFocusApi.AutofacModules
{
    public class MediatorModule
         : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(IMediator).GetTypeInfo().Assembly)
                .AsImplementedInterfaces();

            builder.Register<ServiceFactory>(context =>
            {
                var componentContext = context.Resolve<IComponentContext>();
                return t => { object o; return componentContext.TryResolve(t, out o) ? o : null; };
            });

            builder.RegisterAssemblyTypes(typeof(WorkItemBlockedDomainEventHandler).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(INotificationHandler<>));

            // Product Documentation registrations
            builder.RegisterAssemblyTypes(typeof(ProductDocumentations.Application.CommandHandlers.
                AddProductDocumentation.AddProductDocumentationCommand).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));

            builder.RegisterAssemblyTypes(typeof(GetOrganizationListByUserQuery).GetTypeInfo().Assembly)
              .AsClosedTypesOf(typeof(IRequestHandler<,>));

        }
    }
}
