using Microsoft.EntityFrameworkCore;
using ReviewIQ.AI.Domain;

namespace ReviewIQ.AI.Infrastructure;

public class AiDbContext : DbContext
{
    public AiDbContext(DbContextOptions<AiDbContext> options) : base(options)
    {
    }

    public DbSet<CodeReview> CodeReviews => Set<CodeReview>();
    public DbSet<ReviewComment> ReviewComments => Set<ReviewComment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // CodeReviews table
        modelBuilder.Entity<CodeReview>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.RepositoryId).IsRequired();
            entity.Property(e => e.IncomingEventId).IsRequired();
            entity.Property(e => e.PullRequestNumber).IsRequired();
            entity.Property(e => e.CommitSha).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Status).IsRequired().HasMaxLength(20);
            entity.Property(e => e.TotalComments).IsRequired();
            entity.Property(e => e.QualityScore).IsRequired();
            entity.Property(e => e.GitHubReviewId).HasMaxLength(100);
            entity.Property(e => e.StartedOn).IsRequired();

            entity.HasMany(e => e.Comments)
                  .WithOne()
                  .HasForeignKey(c => c.CodeReviewId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ReviewComments table
        modelBuilder.Entity<ReviewComment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.CodeReviewId).IsRequired();
            entity.Property(e => e.FilePath).IsRequired().HasMaxLength(500);
            entity.Property(e => e.LineNumber).IsRequired();
            entity.Property(e => e.Category)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(20);
            entity.Property(e => e.Severity)
                  .IsRequired()
                  .HasConversion<string>()
                  .HasMaxLength(20);
            entity.Property(e => e.Comment).IsRequired();
            entity.Property(e => e.Suggestion).HasMaxLength(2000);
            entity.Property(e => e.CreatedOn).IsRequired();
        });
    }
}