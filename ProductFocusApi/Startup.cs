using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using ProductFocus.Api.AuthorizationPolicies;
using ProductFocus.DI.Utils;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation.AspNetCore;
using ProductFocusApi.Validations;
using Autofac;
using ProductFocus.Domain.Common;
using System.Collections.Generic;
using ProductFocusApi.StartUp;
using Releases.Application.StartUp;
using BusinessRequirements.Application.StartUp;
using ProductDocumentations.Application.StartUp;
using ProductTests.Application.StartUp;
using IntegrationCommon.Services;
using CommandBus.Abstractions;
using CommandBusServiceBus;
using CommandBusRabbitMQ;
using CommandBus;
using EventBus;
using System;
using EventBus.Abstractions;
using EventBusServiceBus;
using EventBusRabbitMQ;
using FluentValidation;
using ProductFocusApi.CommandHandlers;

namespace ProductFocus.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddProductFocus(Configuration)
                .AddReleases(Configuration)
                .AddBusinessRequirement(Configuration)
                .AddProductDocumentation(Configuration);

            services.AddAutoMapper(typeof(Startup));
            services.AddControllersWithViews();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddMicrosoftIdentityWebApi(options =>
                    {
                        Configuration.Bind("AzureAdB2C", options);
                        options.TokenValidationParameters.NameClaimType = "name";
                    },
            options => { Configuration.Bind("AzureAdB2C", options); });

            services.AddControllers()
                .AddFluentValidation(fv => fv.RegisterValidatorsFromAssemblyContaining<AddSprintCommandValidator>());
        
            
            services.AddAuthorization(options =>
            {
                // Create policy to check for the scope 'read'
                options.AddPolicy("ReadScope",
                    policy => policy.Requirements.Add(new ScopesRequirement("demo.read")));
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Product_Focus_API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Standard authorization using bearer token. Example: Bearer <token>",
                    In = ParameterLocation.Header,
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });
            
            services.AddHandlers();
            services.AddSingleton<Messages>();


            services.AddCommandBuses(Configuration)
                .AddEventBus(Configuration);

        }

        // Register your own things directly with Autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            /*builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new MediatorModule());*/
            builder.AddProductFocus(Configuration);
            builder.AddReleases(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.AddProductFocus();
            app.AddReleases();
            app.AddBusinessRequirements();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product_Focus_API v1"));
            }


            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseCors(x =>
            {
                x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
            });
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }

    public static class ConfigureMicroservices
    {
        public static IServiceCollection AddCommandBuses(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient(typeof(AtomicIntegrationLogService<,,>));

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton(typeof(IPublishOnlyCommandBus<,>), typeof(PublishOnlyCommandBusServiceBus<,>));
                services.AddSingleton(typeof(ICommandBus<>), typeof(CommandBusServiceBus<>));
            }
            else
            {
                services.AddSingleton(typeof(IPublishOnlyCommandBus<,>), typeof(PublishOnlyCommandBusRabbitMQ<,>));
                services.AddSingleton(typeof(ICommandBus<>), typeof(CommandBusRabbitMQ<>));
            }

            services.AddSingleton(typeof(ICommandBusSubscriptionsManager<>), typeof(InMemoryCommandBusSubscriptionsManager<>));


            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            Console.WriteLine("Service bus initialize");
            Console.WriteLine("Azure Service Bus - {0}", configuration.GetValue<bool>("AzureServiceBusEnabled"));

            services.AddSingleton(typeof(IEventBusSubscriptionsManager<>), typeof(InMemoryEventBusSubscriptionsManager<>));

            if (configuration.GetValue<bool>("AzureServiceBusEnabled"))
            {
                services.AddSingleton(typeof(IEventBus<>), typeof(EventBusServiceBus<>));
            }
            else
            {
                services.AddSingleton(typeof(IEventBus<>), typeof(EventBusRabbitMQ<>));
            }

            return services;
        }
    }
}
