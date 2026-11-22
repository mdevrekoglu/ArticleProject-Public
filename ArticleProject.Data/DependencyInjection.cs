using ArticleProject.Data.ArticleProjectDbContext.Repositories;
using ArticleProject.Domain.Common.Interfaces.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ArticleProject.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            // Add all repositories
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IReadOnlyRepository<>), typeof(ReadOnlyRepository<>));
            services.AddScoped(typeof(ILogRepository), typeof(LogRepository));

            return services;
        }
    }
}