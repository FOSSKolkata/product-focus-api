using BusinessRequirements.Domain.Common;
using BusinessRequirements.Domain.Model;
using BusinessRequirements.Domain.Model.BusinessAggregate;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BusinessRequirements.Infrastructure
{
    public class BusinessRequirementDbContext : DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Organization>()
             .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<Product>()
             .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<BusinessRequirement>()
             .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<BusinessRequirement>()
             .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<BusinessRequirementTag>()
             .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<BusinessRequirementAttachment>()
             .Property(o => o.Id).UseHiLo();
            modelBuilder.Entity<Tag>()
             .Property(o => o.Id).UseHiLo();

            modelBuilder.HasDefaultSchema("businessrequirement");
                                                                                                                                 }
        public BusinessRequirementDbContext(DbContextOptions<BusinessRequirementDbContext> options) : base(options)
        {

        }
        public BusinessRequirementDbContext(DbContextOptions<BusinessRequirementDbContext> options, IMediator mediator) : base(options)
        {

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("ProductTestContext::ctor ->" + this.GetHashCode());
        }
        
        public DbSet<BusinessRequirement> BusinessRequirements { get; set; }
        public DbSet<BusinessRequirementTag> BusinessRequirementTags { get; set; }
        public DbSet<BusinessRequirementAttachment> BusinessRequirementAttachments { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Tag> Tags { get; set; }

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
