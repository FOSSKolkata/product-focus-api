using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using ProductFocus.Api.AuthorizationPolicies;
using ProductFocus.Persistence;
using Microsoft.EntityFrameworkCore;
using ProductFocus.DI.Utils;
using ProductFocus.ConnectionString;
using ProductFocus.Services;
using Swashbuckle.AspNetCore.Filters;
using FluentValidation.AspNetCore;
using ProductFocusApi.Validations;
using Autofac;
using ProductFocusApi.AutofacModules;
using Microsoft.Data.SqlClient;
using ProductFocusApi.ConnectionString;
using Azure.Storage.Blobs;
using ProductFocus.Persistence.Services;
using ProductFocus.Domain.Services;
using ProductFocus.Domain.Common;
using System.Collections.Generic;

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

            var builder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("DefaultConnectionAzure"));
            //builder.Password = Configuration["DevDbPassword"];
            //builder.UserID = Configuration["DevDbUser"];
            var connection = Configuration["DefaultConnectionAzure"];
            services.AddDbContext<ProductFocusDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));

            services.AddDbContext<ProductDocumentations.Infrastructure.ProductDocumentationDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));

            services.AddDbContext<ProductTests.Infrastructure.ProductTestDbContext>(
                x => x.UseLazyLoadingProxies()
                    .UseSqlServer(connection));

            ////services.AddDbContext<ProductFocusDbContext>(
            //    x => x.UseLazyLoadingProxies()
            //        .UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddHandlers();
            services.AddSingleton<Messages>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<ProductDocumentations.Infrastructure.UnitOfWork>();
            services.AddTransient<ProductDocumentations.Domain.Common.IUnitOfWork, ProductDocumentations.Infrastructure.UnitOfWork>();
            services.AddTransient<ProductTests.Infrastructure.UnitOfWork>();
            services.AddTransient<ProductTests.Domain.Common.IUnitOfWork, ProductTests.Infrastructure.UnitOfWork>();

            var queryBuilder = new SqlConnectionStringBuilder(
                Configuration.GetConnectionString("QueriesConnectionString"));
            //builder.Password = Configuration["DevDbPassword"];
            //builder.UserID = Configuration["DevDbUser"];
            var queryConnection = Configuration["QueriesConnectionString"];

            var queriesConnectionString = new QueriesConnectionString(queryConnection);
            var blobConnection = Configuration.GetConnectionString("BlobConnectionString");
            var businessRequirementAttachmentContainerName = Configuration.GetConnectionString("BusinessRequirementAttachmentContainerName");
            services.AddSingleton(new BusinessRequirementContainerName(businessRequirementAttachmentContainerName));
            services.AddSingleton(new BlobConnectionString(blobConnection));
            services.AddSingleton(new BlobServiceClient(blobConnection));
            services.AddTransient<IBlobStorageService, BlobStorageService>();
            services.AddSingleton(queriesConnectionString);
            services.AddTransient<IEmailService, EmailService>();
        }

        // Register your own things directly with Autofac
        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new ApplicationModule());
            builder.RegisterModule(new MediatorModule());
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
