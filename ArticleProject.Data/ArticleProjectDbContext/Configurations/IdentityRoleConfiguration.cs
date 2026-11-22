using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ArticleProject.Data.ArticleProjectDbContext.Configurations
{
    public class IdentityRoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole
                {
                    Id = "0b6dc98b-1c8a-4b6f-bc24-f34ad29bfb4d",
                    Name = "Student",
                    NormalizedName = "STUDENT"
                },
                new IdentityRole
                {
                    Id = "8580e89e-c62f-4e37-beeb-e54ddae763c9",
                    Name = "Lecturer",
                    NormalizedName = "LECTURER"
                },
                new IdentityRole
                {
                    Id = "52bbf956-a8b9-45ae-a014-c64328fff122",
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            );
        }
    }
}