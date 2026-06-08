using Microsoft.EntityFrameworkCore;
using ReviewIQ.Gateway.Domain;

namespace ReviewIQ.Gateway.Infrastructure
{
    public class GatewayDbContext : DbContext
    {
        public GatewayDbContext(DbContextOptions<GatewayDbContext> options)
        : base(options)
        {
        }

        public DbSet<IncomingEvent> IncomingEvents => Set<IncomingEvent>();

        public DbSet<Repository> Repositories => Set<Repository>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // IncomingEvent configuration
            modelBuilder.Entity<IncomingEvent>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.DeliveryId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.EventType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Action)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PullRequestTitle)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.PrAuthorLogin)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.RawPayload)
                    .IsRequired();

                entity.HasIndex(e => e.DeliveryId)
                    .IsUnique();
            });


            // Repository configuration
            modelBuilder.Entity<Repository>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.GitHubRepoId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Owner)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.WebhookSecret)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.IsActive)
                    .IsRequired();

                entity.Property(e => e.CreatedOn)
                    .IsRequired();

                entity.HasIndex(e => new { e.Owner, e.Name })
                    .IsUnique();
            });
        }
    }
}
