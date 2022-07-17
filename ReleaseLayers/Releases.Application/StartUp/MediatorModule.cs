using Autofac;
using MediatR;
using Releases.Application.CommandHandler.ReleaseCommands;
using System.Reflection;

namespace Releases.Application.StartUp
{
    public class MediatorModule
         : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(AddReleaseCommand)
                .GetTypeInfo().Assembly).AsClosedTypesOf(typeof(IRequestHandler<,>));
        }
    }
}
