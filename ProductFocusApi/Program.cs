using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace ProductFocus.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder)=>
                { 
                    if(!context.HostingEnvironment.IsDevelopment())
                    {
                        builder.AddSystemsManager("/productfocus");
                    }                    
                }
                )
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                    .UseDefaultServiceProvider(options =>
                        options.ValidateScopes = false);
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory());
    }
}
