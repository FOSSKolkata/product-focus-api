using Autofac;
using Microsoft.Extensions.Configuration;
using Releases.StartUp;

namespace Releases.Application.StartUp
{
    public static class ConfigureAutofacContainer
    {
        public static ContainerBuilder AddReleases(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterModule(new ApplicationModule(configuration["QueriesConnectionString"]));
            builder.RegisterModule(new MediatorModule());

            return builder;
        }
    }
}
