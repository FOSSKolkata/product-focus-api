using Autofac;

namespace Releases.StartUp
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
            
        }
    }
}
