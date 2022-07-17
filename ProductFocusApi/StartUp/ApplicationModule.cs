using Autofac;
using System;
using System.Linq;

namespace ProductFocusApi.StartUp
{
    public class ApplicationModule
          : Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;
        }

        protected override void Load(ContainerBuilder builder)
        {

            //builder.RegisterModule(new ApplicationModule(QueriesConnectionString));
            //builder.RegisterModule(new MediatorModule());
            
            builder.RegisterAssemblyTypes(AppDomain.CurrentDomain.GetAssemblies())
               .Where(t => t.Name.EndsWith("Repository"))
               .AsImplementedInterfaces();
        }
    }
}
