using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class LogEntryConfiguration : IEntityTypeConfiguration<LogEntry>
    {
        public void Configure(EntityTypeBuilder<LogEntry> builder)
        {
            builder.HasKey(p => p.Id);

            builder.Property(p => p.IpAddress).HasMaxLength(50).IsRequired();

            builder.Property(p => p.RequestPath).HasMaxLength(255).IsRequired();

            builder.Property(p => p.HttpMethod).HasMaxLength(10).IsRequired();

            builder.Property(p => p.QueryString).HasMaxLength(500).IsRequired(false);

            builder.Property(p => p.RequestBody).HasMaxLength(5000).IsRequired(false);

            builder.Property(p => p.StackTrace).IsRequired(false);

            builder.Property(p => p.IsError).IsRequired();

            builder.Property(p => p.CreatedDate).IsRequired();

            builder.Property(p => p.UserId).HasMaxLength(450).IsRequired(false);
            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}