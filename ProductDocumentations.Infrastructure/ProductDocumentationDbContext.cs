using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using ProductDocumentations.Domain.Common;
using ProductDocumentations.Domain.Model;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ProductDocumentations.Infrastructure
{
    public class ProductDocumentationDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductDocumentation>()
                .Property(o => o.Id).UseHiLo();

        }
        public ProductDocumentationDbContext(DbContextOptions<ProductDocumentationDbContext> options) : base(options)
        {

        }
        public ProductDocumentationDbContext(DbContextOptions<ProductDocumentationDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());

        }

        public DbSet<ProductDocumentation> ProductDocumentations { get; set; }

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

    public class ProductDocumentationContextFactory : IDesignTimeDbContextFactory<ProductDocumentationDbContext>
    {
        public ProductDocumentationDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<ProductDocumentationDbContext>();
            optionsBuilder.UseSqlServer("Server=localhost\\SQLEXPRESS; Database=product-focus; Trusted_Connection=True;");

            return new ProductDocumentationDbContext(optionsBuilder.Options, new NoMediator());
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
