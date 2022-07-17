using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace IntegrationEventLogEF
{
    public class IntegrationEventLogContext : DbContext
    {
        string _schema;
        public IntegrationEventLogContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _schema = configuration["DefaultDBSchema"];
        }

        public IntegrationEventLogContext(DbContextOptions options, string schema) : base(options)
        {
            _schema = schema;
        }

        public DbSet<IntegrationEventLogEntry> IntegrationEventLogs { get; set; }
        public DbSet<IncomingIntegrationEventLogEntry> IncomingIntegrationEventLogs { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationEventLogEntry>(ConfigureIntegrationEventLogEntry);
            builder.Entity<IncomingIntegrationEventLogEntry>(ConfigureIncomingIntegrationEventLogEntry);
         
            if (!string.IsNullOrEmpty(_schema))
                builder.HasDefaultSchema(_schema);
        }

        void ConfigureIntegrationEventLogEntry(EntityTypeBuilder<IntegrationEventLogEntry> builder)
        {
            builder.ToTable("IntegrationEventLog");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

        }

        void ConfigureIncomingIntegrationEventLogEntry(EntityTypeBuilder<IncomingIntegrationEventLogEntry> builder)
        {
            builder.ToTable("IncomingIntegrationEventLog");

            builder.HasKey(e => e.EventId);

            builder.Property(e => e.EventId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesReceived)
                .IsRequired();

            builder.Property(e => e.EventTypeName)
                .IsRequired();

            builder.Property(e => e.ExceptionMessage).HasMaxLength(500);
        }
    }
}
