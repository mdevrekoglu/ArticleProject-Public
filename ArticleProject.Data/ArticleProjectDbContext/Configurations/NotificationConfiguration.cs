using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class NotificationConfiguration : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(500);

            builder.Property(p => p.Description).IsRequired().HasMaxLength(5000);

            builder.Property(p => p.IsActive).IsRequired();

            builder.Property(propertyExpression: p => p.CreatedBy).IsRequired();
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}