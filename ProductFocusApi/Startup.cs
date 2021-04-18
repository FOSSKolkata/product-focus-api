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

            services.AddControllers();
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

            services.AddDbContext<ProductFocusDbContext>(x => x.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHandlers();
            services.AddSingleton<Messages>();
            services.AddTransient<IOrganizationRepository, OrganizationRepository>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            var queriesConnectionString = new QueriesConnectionString(Configuration.GetConnectionString("QueriesConnectionString"));
            services.AddSingleton(queriesConnectionString);
            services.AddTransient<IEmailService, EmailService>();
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
