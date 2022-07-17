using IntegrationCommandLogEF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Configuration;


namespace IntegrationCommandLogEF
{
    public class IntegrationCommandLogContext : DbContext
    {
        string _schema;
        public IntegrationCommandLogContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _schema = configuration["DefaultDBSchema"];
        }

        public IntegrationCommandLogContext(DbContextOptions options, string schema) : base(options)
        {
            _schema = schema;
        }

        public DbSet<IntegrationCommandLogEntry> IntegrationCommandLogs { get; set; }

        public DbSet<IncomingIntegrationCommandLogEntry> IncomingIntegrationCommandLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IntegrationCommandLogEntry>(ConfigureIntegrationCommandLogEntry);
            builder.Entity<IncomingIntegrationCommandLogEntry>(ConfigureIncomingIntegrationCommandLogEntry);

            if (!string.IsNullOrEmpty(_schema))
                builder.HasDefaultSchema(_schema);
        }

        void ConfigureIntegrationCommandLogEntry(EntityTypeBuilder<IntegrationCommandLogEntry> builder)
        {
            builder.ToTable("IntegrationCommandLog");

            builder.HasKey(e => e.CommandId);

            builder.Property(e => e.CommandId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesSent)
                .IsRequired();

            builder.Property(e => e.CommandTypeName)
                .IsRequired();

        }
        void ConfigureIncomingIntegrationCommandLogEntry(EntityTypeBuilder<IncomingIntegrationCommandLogEntry> builder)
        {
            builder.ToTable("IncomingIntegrationCommandLog");

            builder.HasKey(e => e.CommandId);

            builder.Property(e => e.CommandId)
                .IsRequired();

            builder.Property(e => e.Content)
                .IsRequired();

            builder.Property(e => e.CreationTime)
                .IsRequired();

            builder.Property(e => e.State)
                .IsRequired();

            builder.Property(e => e.TimesReceived)
                .IsRequired();

            builder.Property(e => e.CommandTypeName)
                .IsRequired();

            builder.Property(e => e.ExceptionMessage).HasMaxLength(500);
        }
    }
}
