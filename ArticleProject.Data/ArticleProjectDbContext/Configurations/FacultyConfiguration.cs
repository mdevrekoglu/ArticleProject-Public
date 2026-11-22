using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class FacultyConfiguration : IEntityTypeConfiguration<Faculty>
    {
        public void Configure(EntityTypeBuilder<Faculty> builder)
        {
            builder.Property(p => p.FacultyName).IsRequired().HasMaxLength(100);

            builder.Property(p => p.AffiliationId).IsRequired();

            // Define the foreign key relationship between MajorDiscipline and Department
            builder.HasOne(p => p.Affiliation)
                   .WithMany()
                   .HasForeignKey(p => p.AffiliationId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}