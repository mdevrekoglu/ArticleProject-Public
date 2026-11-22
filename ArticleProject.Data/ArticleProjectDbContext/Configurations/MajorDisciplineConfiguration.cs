using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class MajorDisciplineConfiguration : IEntityTypeConfiguration<MajorDiscipline>
    {
        public void Configure(EntityTypeBuilder<MajorDiscipline> builder)
        {
            // Optionally, you can configure other properties like string length, nullability, etc.
            builder.Property(p => p.MajorDisciplineName).IsRequired().HasMaxLength(100);

            builder.Property(p => p.DepartmentId).IsRequired();

            // Define the foreign key relationship between MajorDiscipline and Department
            builder.HasOne(p => p.Department)
                   .WithMany()
                   .HasForeignKey(p => p.DepartmentId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}