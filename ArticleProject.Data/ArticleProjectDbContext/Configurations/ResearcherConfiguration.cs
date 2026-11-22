using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class ResearcherConfiguration : IEntityTypeConfiguration<Researcher>
    {
        public void Configure(EntityTypeBuilder<Researcher> builder)
        {
            builder.Property(p => p.IsActive).HasDefaultValue(true).IsRequired();

            builder.Property(p => p.FirstName).IsRequired(false).HasMaxLength(100);

            builder.Property(p => p.LastName).IsRequired(false).HasMaxLength(200);

            builder.Property(p => p.Position).IsRequired(false).HasMaxLength(50);

            builder.Property(p => p.StaffTitle).IsRequired(false).HasMaxLength(50);

            builder.Property(p => p.DocumentCount).HasDefaultValue(0).IsRequired(false);

            builder.Property(p => p.Email).IsRequired(false).HasMaxLength(256);
            builder.HasIndex(p => p.Email).IsUnique().HasFilter("[Email] IS NOT NULL");

            builder.Property(p => p.WoSId).IsRequired(false).HasMaxLength(20);
            builder.HasIndex(p => p.WoSId).IsUnique().HasFilter("[WoSId] IS NOT NULL");

            builder.Property(p => p.ScopusId).IsRequired();
            builder.HasIndex(p => p.ScopusId).IsUnique().HasFilter("[ScopusId] IS NOT NULL");

            builder.Property(p => p.ORCId).IsRequired(false).HasMaxLength(19);
            builder.HasIndex(p => p.ORCId).IsUnique().HasFilter("[ORCId] IS NOT NULL");

            builder.Property(p => p.AffiliationId).IsRequired();
            builder.HasOne(p => p.Affiliation)
                   .WithMany()
                   .HasForeignKey(p => p.AffiliationId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.MajorDisciplineId).IsRequired(false);
            builder.HasOne(p => p.MajorDiscipline)
                   .WithMany()
                   .HasForeignKey(p => p.MajorDisciplineId)
                   .OnDelete(DeleteBehavior.NoAction);
        }
    }
}