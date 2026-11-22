using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);

            builder.Property(p => p.Quartile).IsRequired(false);

            builder.Property(p => p.Rank).IsRequired(false);

            builder.Property(p => p.TotalJournals).IsRequired(false);
        }
    }
}