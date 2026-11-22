using ArticleProject.Domain.Aggregates;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ArticleProject.Data.ArticleProjectDbContext
{
    public class ArticleProjectContext : IdentityDbContext<User>
    {
        public ArticleProjectContext(DbContextOptions<ArticleProjectContext> options) : base(options)
        {
        }

        public DbSet<Affiliation> Affiliations { get; set; }
        public DbSet<Department> Departments { get; set; }
        public DbSet<Faculty> Faculties { get; set; }
        public DbSet<MajorDiscipline> MajorDisciplines { get; set; }
        public DbSet<Researcher> Researchers { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Journal> Journals { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<NotificationTarget> NotificationTargets { get; set; }
        public DbSet<UserImage> UserImages { get; set; }
        public DbSet<AzureFile> AzureFiles { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(typeof(ArticleProjectContext).Assembly);
        }
    }
}