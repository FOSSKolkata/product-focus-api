using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductFocus.Api.AuthorizationPolicies;
using Common;
using ProductFocus.Persistence;
using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Repositories;
using ProductFocus.Persistence.Repositories;
using ProductFocus.Domain;
using ProductFocus.DI.Utils;
using ProductFocus.AppServices;
using ProductFocus.ConnectionString;
using ProductFocus.Services;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation.AspNetCore;
using ProductFocusApi.Validations;
using Autofac;
using ProductFocusApi.AutofacModules;
using Microsoft.Data.SqlClient;

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
            });

            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("DefaultConnection"));
            //builder.Password = Configuration["DevDbPassword"];
            //builder.UserID = Configuration["DevDbUser"];
            var connection = builder.ConnectionString;

            services.AddDbContext<ProductFocusDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));

            ////services.AddDbContext<ProductFocusDbContext>(
            //    x => x.UseLazyLoadingProxies()
            //        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHandlers();
            services.AddSingleton<Messages>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();

            var queryBuilder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("QueriesConnectionString"));
            //builder.Password = Configuration["DevDbPassword"];
            //builder.UserID = Configuration["DevDbUser"];
            var queryConnection = builder.ConnectionString;

            var queriesConnectionString = new QueriesConnectionString(queryConnection);
            
            services.AddSingleton(queriesConnectionString);
            services.AddTransient<IEmailService, EmailService>();
        }

        // Register your own things directly with Autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
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
}
