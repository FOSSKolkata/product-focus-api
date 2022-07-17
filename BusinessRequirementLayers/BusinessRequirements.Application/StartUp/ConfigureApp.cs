using CommandBus.Abstractions;
using EventBus.Abstractions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessRequirements.Application.StartUp
{
    public static class ConfigureApp
    {
        public static IApplicationBuilder AddBusinessRequirements(this IApplicationBuilder app)
        {
            //var commandBus = app.ApplicationServices.GetRequiredService<ICommandBus<>>

            //var eventBus = app.ApplicationServices.GetRequiredService<IEventBus<>>
            return app;
        }
    }
}
