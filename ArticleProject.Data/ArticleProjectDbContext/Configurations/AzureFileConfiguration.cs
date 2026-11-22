using ArticleProject.Domain.Aggregates;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContept.Configurations
{
    public class AzureFileConfiguration : IEntityTypeConfiguration<AzureFile>
    {
        public void Configure(EntityTypeBuilder<AzureFile> builder)
        {
            builder.Property(p => p.GUID)
                .IsRequired()
                .HasMaxLength(100);
            builder.HasIndex(p => p.GUID)
                .IsUnique();

            builder.Property(p => p.FileName)
                .IsRequired()
                .HasMaxLength(255);
            builder.HasIndex(p => p.FileName)
                .IsUnique();

            builder.Property(p => p.Description)
                .HasMaxLength(500);

            builder.Property(p => p.FileType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(p => p.ReportType)
                .HasConversion<int>()
                .IsRequired();

            builder.Property(propertyExpression: p => p.CreatedBy).IsRequired();
            builder.HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}