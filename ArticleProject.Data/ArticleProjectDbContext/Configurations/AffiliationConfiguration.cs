using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class AffiliationConfiguration : IEntityTypeConfiguration<Affiliation>
    {
        public void Configure(EntityTypeBuilder<Affiliation> builder)
        {
            builder.Property(p => p.AffiliationName).HasMaxLength(100).IsRequired();
            builder.HasIndex(p => p.AffiliationName).IsUnique();

            builder.Property(p => p.Address).HasMaxLength(200).IsRequired();

            builder.Property(p => p.City).HasMaxLength(50).IsRequired();

            builder.Property(p => p.Country).HasMaxLength(50).IsRequired();

            builder.Property(p => p.ScopusId).IsRequired();
            builder.HasIndex(p => p.ScopusId).IsUnique();

            builder.Property(p => p.OrganizationType).HasMaxLength(50).IsRequired();
        }
    }
}