using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductFocus.Domain.Common;
using ProductFocus.Domain.Model;
using ProductFocus.Domain.Model.FeatureAggregate;

namespace ProductFocus.Persistence
{
    public class ProductFocusDbContext: DbContext
    {
        protected override void OnModelCreating(ModelBuilder modelbuilder)
        {
            modelbuilder.Entity<Feature>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<FeatureComment>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<ScrumDay>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Domain.Model.Task>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Member>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Organization>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Module>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Product>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Role>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<RolePermission>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Invitation>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Permission>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Sprint>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<User>(entity =>
            {
                entity.Property(o => o.Id).UseHiLo();
                entity.HasIndex(o => o.ObjectId).IsUnique();
            });
            modelbuilder.Entity<UserToFeatureAssignment>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Tag>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<TagCategory>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<Release>()
             .Property(o => o.Id).UseHiLo();
            modelbuilder.Entity<CurrentProgressWorkItem>()
             .Property(o => o.Id).UseHiLo();
        }
        public ProductFocusDbContext(DbContextOptions<ProductFocusDbContext> options) : base(options)
        {

        }

        public ProductFocusDbContext(DbContextOptions<ProductFocusDbContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));


            System.Diagnostics.Debug.WriteLine("OrderingContext::ctor ->" + this.GetHashCode());
        }

        public DbSet<Feature> Features { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Invitation> Invitations { get; set; }
        public DbSet<WorkItemDomainEventLog> WorkItemDomainEventLogs { get; set; }
        public DbSet<FeatureOrdering> FeatureOrders { get; set; }
        public DbSet<TagCategory> TagCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Release> Releases { get; set; }
        public DbSet<CurrentProgressWorkItem> CurrentProgressWorkItems { get; set; }

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
