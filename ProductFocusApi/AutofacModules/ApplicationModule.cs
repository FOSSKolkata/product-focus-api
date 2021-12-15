using Autofac;
using System;
using System.Linq;

namespace ProductFocusApi.AutofacModules
{
    public class ApplicationModule
          : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces();
        }
    }
}
