using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.FirstName).HasMaxLength(30).IsRequired();

            builder.Property(p => p.LastName).HasMaxLength(20).IsRequired();

            builder.Property(p => p.RefreshToken).HasMaxLength(128).IsRequired(false);

            builder.Property(p => p.RefreshTokenExpiryDate).IsRequired(false);

            builder.Property(p => p.CreatedDate).IsRequired();

            builder.Property(p => p.ResearcherId).IsRequired(false);
            builder.HasOne(p => p.Researcher)
                .WithOne(r => r.User)
                .HasForeignKey<User>(p => p.ResearcherId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Property(p => p.DepartmentId).IsRequired(false);
            builder.HasOne(p => p.Department)
                .WithMany()
                .HasForeignKey(p => p.DepartmentId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(p => p.UserRoles)
                 .WithOne()
                 .HasForeignKey(p => p.UserId)
                 .IsRequired();

            builder.Property(p => p.ImageId).IsRequired(false);
            builder.HasOne(p => p.Image)
                .WithMany()
                .HasForeignKey(p => p.ImageId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}