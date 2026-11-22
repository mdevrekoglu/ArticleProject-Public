using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class JournalConfiguration : IEntityTypeConfiguration<Journal>
    {
        public void Configure(EntityTypeBuilder<Journal> builder)
        {
            builder.Property(p => p.Name).IsRequired().HasMaxLength(200);
            builder.HasIndex(p => p.Name);

            builder.Property(p => p.AbbreviationName).IsRequired().HasMaxLength(100);
            builder.HasIndex(p => p.AbbreviationName);

            builder.Property(p => p.ISSN).IsRequired(false).HasMaxLength(20);
            builder.HasIndex(p => p.ISSN);

            builder.Property(p => p.EISSN).IsRequired(false).HasMaxLength(20);
            builder.HasIndex(p => p.EISSN);

            builder.Property(p => p.JIF).IsRequired(false).HasPrecision(6, 2);

            builder.Property(p => p.JIF5Years).IsRequired(false).HasPrecision(6, 2);

            builder.Property(p => p.Year).IsRequired();

            builder.HasOne(p => p.Category)
                   .WithMany()
                   .HasForeignKey(p => p.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(j => new { j.ISSN, j.Year }).IsUnique();

            builder.HasIndex(j => new { j.EISSN, j.Year }).IsUnique();
        }
    }
}