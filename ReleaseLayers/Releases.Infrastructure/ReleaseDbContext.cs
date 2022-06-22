using MediatR;
using Microsoft.EntityFrameworkCore;
using Releases.Domain.Common;
using Releases.Domain.Model;

namespace Releases.Infrastructure
{
    public class ReleaseDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Release>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<ReleaseWorkItemCount>()
                .Property(o => o.Id).UseHiLo();
            modelBuilder.HasDefaultSchema("release");
        }
        public ReleaseDbContext(DbContextOptions<ReleaseDbContext> options) : base(options)
        {

        }
        public ReleaseDbContext(DbContextOptions<ReleaseDbContext> options, IMediator mediator) : base(options)
        {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("ProductTestContext::ctor ->" + this.GetHashCode());
        }

        public DbSet<Release> Releases { get; set; }
        public DbSet<ReleaseWorkItemCount> ReleaseWorkItemCounts { get; set; }

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
}
