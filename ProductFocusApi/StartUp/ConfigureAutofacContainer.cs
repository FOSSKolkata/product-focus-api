using Autofac;
using Microsoft.Extensions.Configuration;

namespace ProductFocusApi.StartUp
{
    public static class ConfigureAutofacContainer
    {
        public static ContainerBuilder AddProductFocus(this ContainerBuilder builder, IConfiguration configuration)
        {
            builder.RegisterModule(new ApplicationModule(configuration["QueriesConnectionString"]));
            builder.RegisterModule(new MediatorModule());

            return builder;
        }
    }
}
