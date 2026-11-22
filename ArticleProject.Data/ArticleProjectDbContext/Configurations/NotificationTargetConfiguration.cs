using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class NotificationTargetConfiguration : IEntityTypeConfiguration<NotificationTarget>
    {
        public void Configure(EntityTypeBuilder<NotificationTarget> builder)
        {
            builder.Property(p => p.IsRead).IsRequired();

            builder.Property(p => p.UserId).IsRequired();
            builder.HasIndex(p => p.UserId);
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.NotificationId).IsRequired();
            builder.HasIndex(p => p.NotificationId);
            builder.HasOne(p => p.Notification)
             .WithMany(p => p.NotificationTargets)
             .HasForeignKey(p => p.NotificationId)
             .OnDelete(DeleteBehavior.NoAction);
        }
    }
}