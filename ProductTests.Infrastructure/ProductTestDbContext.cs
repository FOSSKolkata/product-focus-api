using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductTests.Domain.Common;
using ProductTests.Domain.Model.TestCaseAggregate;
using ProductTests.Domain.Model.TestCaseVersionAggregate;
using ProductTests.Domain.Model.TestPlanAggregate;
using ProductTests.Domain.Model.TestPlanVersionAggregate;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductTests.Infrastructure
{
    public class ProductTestDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TestPlan>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestSuite>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestCase>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestStep>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestSuiteTestCaseMapping>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestPlanVersion>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestSuiteVersion>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestCaseVersion>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestStepVersion>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<TestSuiteTestCaseMappingVersion>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.HasDefaultSchema("producttest");

        }
        public ProductTestDbContext(DbContextOptions<ProductTestDbContext> options) : base(options)
        {

        }
        public ProductTestDbContext(DbContextOptions<ProductTestDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("ProductTestContext::ctor ->" + this.GetHashCode());

        }

        public DbSet<TestPlan> TestPlans { get; set; }
        public DbSet<TestSuite> TestSuites { get; set; }
        public DbSet<TestCase> TestCases { get; set; }
        public DbSet<TestStep> TestSteps { get; set; }
        public DbSet<TestSuiteTestCaseMapping> TestSuiteTestCaseMappings { get; set; }
        public DbSet<TestPlanVersion> TestPlansVersion { get; set; }
        public DbSet<TestSuiteVersion> TestSuitesVersion { get; set; }
        public DbSet<TestCaseVersion> TestCasesVersion { get; set; }
        public DbSet<TestStepVersion> TestStepsVersion { get; set; }
        public DbSet<TestSuiteTestCaseMappingVersion> TestSuiteTestCaseMappingsVersion { get; set; }

        private readonly IMediator _mediator;
        public async Task<int> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            // Dispatch Domain Events collection. 
            // Choices:
            // A) Right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            // B) Right AFTER committing data (EF SaveChanges) into the DB will make multiple transactions. 
            // You will need to handle eventual consistency and compensatory actions in case of failures in any of the Handlers. 
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync(cancellationToken);

            return result;
        }
    }

    public class ProductTestContextFactory : IDesignTimeDbContextFactory<ProductTestDbContext>
    {
        public ProductTestDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductTestDbContext>();
            optionsBuilder.UseSqlServer("Server=tcp:productfocus01.database.windows.net,1433;Initial Catalog=productfocus-db;Persist Security Info=False;User ID=azureadmin;Password=P@ssw0rd;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

            return new ProductTestDbContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default)
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult<TResponse>(default);
            }

            public Task<object> Send(object request, CancellationToken cancellationToken = default)
            {
                return Task.FromResult(default(object));
            }
        }
    }
}
