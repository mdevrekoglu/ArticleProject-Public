using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class DepartmentConfiguration : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.Property(p => p.DepartmentName).IsRequired().HasMaxLength(100);

            builder.Property(p => p.FacultyId).IsRequired();

            builder.Property(p => p.StudentCount).IsRequired();

            builder.Property(p => p.ResearcherCount).IsRequired();

            // Define the foreign key relationship between Faculty and Department
            builder.HasOne(p => p.Faculty)
                   .WithMany()
                   .HasForeignKey(p => p.FacultyId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}